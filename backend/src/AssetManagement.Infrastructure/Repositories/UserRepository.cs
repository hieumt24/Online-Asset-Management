using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AssetManagement.Infrastructure.Repositories
{
    public class UserRepository : BaseRepositoryAsync<User>, IUserRepositoriesAsync
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            var user = await _dbContext.Users
           .Where(u => u.Username.ToLower() == username.ToLower() && !u.IsDeleted)
           .FirstOrDefaultAsync();

            if (user != null && user.Username.Equals(username, StringComparison.Ordinal))
            {
                return user;
            }

            return null;
        }

        public string GeneratePassword(string userName, DateTime dateOfBirth)
        {
            string dob = dateOfBirth.ToString("ddMMyyyy");
            return $"{userName}@{dob}";
        }

        public async Task<string> GenerateUsername(string firstName, string lastName)
        {
            // Normalize names to lower case
            firstName = firstName.ToLower().Replace(" ", "");
            lastName = string.Join(" ", lastName.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries));

            // Get the first letter of each part of the last name
            var lastNameParts = lastName.Split(' ');
            var lastNameInitials = string.Join("", lastNameParts.Select(part => part[0]));

            // Combine first name and initials of last names

            string baseUserName = firstName + lastNameInitials;
            return await GenerateUniqueUserName(baseUserName);
        }

        private async Task<string> GenerateUniqueUserName(string baseUserName)
        {
            var matchingUserNames = _dbContext.Users
                .Where(u => u.Username.StartsWith(baseUserName)).
                Select(u => u.Username).ToList();
            if (matchingUserNames.Count == 0)
            {
                return baseUserName;
            }
            int maxNumber = 0;

            foreach (var userName in matchingUserNames)
            {
                string numericPart = userName.Substring(baseUserName.Length);
                if (int.TryParse(numericPart, out int number))
                {
                    if (number > maxNumber)
                    {
                        maxNumber = number;
                    }
                }
            }

            return baseUserName + (maxNumber + 1);
        }

        public async Task<RoleType> GetRoleAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user.Role;
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public IQueryable<User> Query(EnumLocation adminLocation)
        {
            return _dbContext.Users.Where(x => x.Location == adminLocation && !x.IsDeleted);
        }

        public async Task<IQueryable<User>> FilterUserAsync(EnumLocation adminLocation, string? search, RoleType? roleType)
        {
            //check adminlocatin
            var query = _dbContext.Users.Where(x => x.Location == adminLocation && !x.IsDeleted);

            //search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(search.ToLower())
                                                        || x.LastName.ToLower().Contains(search.ToLower())
                                                        || x.Username.ToLower().Contains(search.ToLower())
                                                        || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(search.ToLower())
                                                        || x.StaffCode.ToLower().Contains(search.ToLower())
                                                        );
            }

            //filter by role

            if (roleType != null)
            {
                query = query.Where(x => x.Role == roleType);
            }

            return query;
        }

        public async Task<User> UpdateUserAysnc(User user)
        {
            var existingUser = await _dbContext.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            _dbContext.Entry(existingUser).Property(e => e.StaffCodeId).IsModified = false;

            await _dbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<(IEnumerable<User> Data, int TotalRecords)> GetAllMatchingUserAsync(EnumLocation admimLocation, string? search, RoleType? roleType, string? orderBy, bool? isDescending, PaginationFilter pagination)
        {
            string searchPhraseLower = search?.ToLower() ?? string.Empty;

            var baseQuery = _dbContext.Users.AsNoTracking()
                .Where(x => x.Location == admimLocation && !x.IsDeleted);

            var searchQuery = baseQuery.Where(x => x.FirstName.ToLower().Contains(searchPhraseLower)
                                                                        || x.LastName.ToLower().Contains(searchPhraseLower)
                                                                        || x.Username.ToLower().Contains(searchPhraseLower)
                                                                        || (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(searchPhraseLower)
                                                                        || x.StaffCode.ToLower().Contains(searchPhraseLower)
                                                                        );

            if (roleType.HasValue)
            {
                searchQuery = searchQuery.Where(x => x.Role == roleType);
            }

            var totalRecords = await searchQuery.CountAsync();

            if (!string.IsNullOrEmpty(orderBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
                {
                    { "fullname", x => x.FirstName + " " + x.LastName },
                    { "staffcode", x => x.StaffCode },
                    { "username", x => x.Username },
                    { "joineddate", x => x.JoinedDate },
                    { "role", x => x.Role },
                    {"createdon", x => x.CreatedOn },
                    { "lastmodifiedon", x => x.LastModifiedOn }
                };

                if (columnsSelector.ContainsKey(orderBy.ToLower()))
                {
                    if (isDescending.HasValue && isDescending.Value)
                    {
                        searchQuery = searchQuery.OrderByDescending(columnsSelector[orderBy.ToLower()])
                            .ThenByDescending(x => x.FirstName + " " + x.LastName)
                            .ThenByDescending(x => x.StaffCode)
                            .ThenByDescending(x => x.Username)
                            .ThenByDescending(x => x.JoinedDate)
                            .ThenByDescending(x => x.Role);
                    }
                    else
                    {
                        searchQuery = searchQuery.OrderBy(columnsSelector[orderBy.ToLower()])
                            .ThenBy(x => x.FirstName + " " + x.LastName)
                            .ThenBy(x => x.StaffCode)
                            .ThenBy(x => x.Username)
                            .ThenBy(x => x.JoinedDate)
                            .ThenBy(x => x.Role);
                    }
                }
            }
            else
            {
                searchQuery = searchQuery.OrderBy(x => x.FirstName + " " + x.LastName)
                    .ThenBy(x => x.StaffCode)
                    .ThenBy(x => x.Username)
                    .ThenBy(x => x.JoinedDate)
                    .ThenBy(x => x.Role);
            }

            var users = await searchQuery
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (Data: users, TotalRecords: totalRecords);
        }

        private IOrderedQueryable<User> ApplySorting(IQueryable<User> query, string? orderBy, bool? isDescending)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
        {
            { "fullname", x => x.FirstName + " " + x.LastName },
            { "staffcode", x => x.StaffCode },
            { "username", x => x.Username },
            { "joineddate", x => x.JoinedDate },
            { "role", x => x.Role }
        };

            IOrderedQueryable<User> orderedQuery;

            if (!string.IsNullOrEmpty(orderBy) && columnsSelector.ContainsKey(orderBy.ToLower()))
            {
                if (isDescending ?? false)
                {
                    orderedQuery = query.OrderByDescending(columnsSelector[orderBy.ToLower()]);
                }
                else
                {
                    orderedQuery = query.OrderBy(columnsSelector[orderBy.ToLower()]);
                }
            }
            else
            {
                orderedQuery = query.OrderBy(x => x.FirstName + " " + x.LastName);
            }

            return orderedQuery
                .ThenBy(x => x.FirstName + " " + x.LastName)
                .ThenBy(x => x.StaffCode)
                .ThenBy(x => x.Username)
                .ThenBy(x => x.JoinedDate)
                .ThenBy(x => x.Role);
        }
    }
}