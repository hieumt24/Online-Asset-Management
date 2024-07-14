using AssetManagement.Application.Helper;
using AssetManagement.Application.Models.DTOs.Reports.Responses;
using FluentAssertions;

namespace AssetManagement.Application.Tests.Helper
{
    public class ReportHelperTests
    {
        [Fact]
        public void ApplySorting_ShouldSortByCategoryNameAscending_WhenOrderByIsNull()
        {
            // Arrange
            var reports = new List<ReportResponseDto>
            {
                new ReportResponseDto { CategoryName = "B" },
                new ReportResponseDto { CategoryName = "A" }
            };

            // Act
            var result = ReportHelper.ApplySorting(reports, null, null);

            // Assert
            result.Should().BeInAscendingOrder(r => r.CategoryName);
        }

        [Fact]
        public void ApplySorting_ShouldSortByCategoryNameDescending_WhenOrderByIsNullAndIsDescendingIsTrue()
        {
            // Arrange
            var reports = new List<ReportResponseDto>
            {
                new ReportResponseDto { CategoryName = "A" },
                new ReportResponseDto { CategoryName = "B" }
            };

            // Act
            var result = ReportHelper.ApplySorting(reports, null, true);

            // Assert
            result.Should().BeInDescendingOrder(r => r.CategoryName);
        }

        [Fact]
        public void ApplySorting_ShouldSortBySpecifiedPropertyAscending()
        {
            // Arrange
            var reports = new List<ReportResponseDto>
            {
                new ReportResponseDto { CategoryName = "dzung" },
                new ReportResponseDto {  CategoryName = "dzung1" }
            };

            // Act
            var result = ReportHelper.ApplySorting(reports, "TotalAssets", false);

            // Assert
            result.Should().BeInAscendingOrder(r => r.CategoryName);
        }

        [Fact]
        public void ApplySorting_ShouldSortBySpecifiedPropertyDescending()
        {
            // Arrange
            var reports = new List<ReportResponseDto>
            {
                new ReportResponseDto { CategoryName = "dzung" },
                new ReportResponseDto {  CategoryName = "dzung1" }
            };

            // Act
            var result = ReportHelper.ApplySorting(reports, "TotalAssets", true);

            // Assert
            result.Should().BeInDescendingOrder(r => r.CategoryName);
        }

        [Fact]
        public void ApplySorting_ShouldReturnUnsortedList_WhenOrderByIsInvalid()
        {
            // Arrange
            var reports = new List<ReportResponseDto>
            {
                new ReportResponseDto { CategoryName = "A" },
                new ReportResponseDto { CategoryName = "B" }
            };

            // Act
            var result = ReportHelper.ApplySorting(reports, "InvalidProperty", true);

            // Assert
            result.Should().BeEquivalentTo(reports);
        }
    }
}
