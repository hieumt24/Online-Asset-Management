using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Wrappers;

namespace AssetManagement.Application.Helper
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageIndex, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageIndex >= 1 && validFilter.PageIndex < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageIndex + 1, validFilter.PageSize), route)
                : null;
            respose.PreviousPage =
                validFilter.PageIndex - 1 >= 1 && validFilter.PageIndex <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageIndex - 1, validFilter.PageSize), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}