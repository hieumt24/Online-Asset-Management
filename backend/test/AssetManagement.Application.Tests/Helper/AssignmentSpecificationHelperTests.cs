using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Domain.Entites;
using FluentAssertions;
using System.Linq.Expressions;

namespace AssetManagement.Application.Tests.Helper
{
    public class AssignmentSpecificationHelperTests
    {
        [Fact]
        public void AssignmentSpecificationWithAsset_ShouldApplyOrderingAndPaging()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 10 };
            string orderBy = "assignedto";
            bool isDescending = true;

            // Act
            var spec = AssignmentSpecificationHelper.AssignmentSpecificationWithAsset(paginationFilter, orderBy, isDescending);

            // Assert
            spec.Includes.Should().ContainSingle(include => include.ToString().Contains("Asset"));
            spec.Includes.Should().ContainSingle(include => include.ToString().Contains("AssignedBy"));
            spec.Includes.Should().ContainSingle(include => include.ToString().Contains("AssignedTo"));
            spec.OrderByDescending.Should().NotBeNull();
            spec.OrderByDescending.Compile()(new Assignment()).Should().BeAssignableTo<IComparable>();
            spec.Skip.Should().Be(paginationFilter.PageSize * (paginationFilter.PageIndex - 1));
            spec.Take.Should().Be(paginationFilter.PageSize);
        }

        [Fact]
        public void GetOrderByExpression_ShouldReturnCorrectExpression()
        {
            // Arrange & Act
            var orderByExpressions = new[]
            {
                AssignmentSpecificationHelper.GetOrderByExpression("assetcode"),
                AssignmentSpecificationHelper.GetOrderByExpression("assetname"),
                AssignmentSpecificationHelper.GetOrderByExpression("assignedto"),
                AssignmentSpecificationHelper.GetOrderByExpression("assignedby"),
                AssignmentSpecificationHelper.GetOrderByExpression("assigneddate"),
                AssignmentSpecificationHelper.GetOrderByExpression("createdon"),
                AssignmentSpecificationHelper.GetOrderByExpression("lastmodifiedon"),
                AssignmentSpecificationHelper.GetOrderByExpression("state")
            };

            // Assert
            orderByExpressions.Should().AllBeAssignableTo<Expression<Func<Assignment, object>>>();
        }

        [Fact]
        public void AssignmentSpecificationWithAsset_DefaultOrderBy_ShouldBeCreatedOnDescending()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 10 };

            // Act
            var spec = AssignmentSpecificationHelper.AssignmentSpecificationWithAsset(paginationFilter, null, null);

            // Assert
            spec.OrderByDescending.Should().NotBeNull();
            spec.OrderByDescending.Compile()(new Assignment { CreatedOn = DateTime.UtcNow }).Should().BeAssignableTo<IComparable>();
        }

        [Fact]
        public void AssignmentSpecificationWithAsset_WithOrderBy_ShouldApplyCorrectOrder()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 10 };
            string orderBy = "assigneddate";
            bool isDescending = false;

            // Act
            var spec = AssignmentSpecificationHelper.AssignmentSpecificationWithAsset(paginationFilter, orderBy, isDescending);

            // Assert
            spec.OrderBy.Should().NotBeNull();
            spec.OrderBy.Compile()(new Assignment { AssignedDate = DateTime.UtcNow }).Should().BeAssignableTo<IComparable>();
        }
    }
}
