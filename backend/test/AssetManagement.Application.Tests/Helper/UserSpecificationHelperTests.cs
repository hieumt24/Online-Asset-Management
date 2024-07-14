using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using System;
using System.Linq.Expressions;
using Xunit;

namespace AssetManagement.Application.Tests.Helper
{
    public class UserSpecificationHelperTests
    {
        [Fact]
        public void CreateSpecification_ValidInputs_ReturnsSpecificationWithCriteria()
        {
            // Arrange
            var paginationFilter = new PaginationFilter(1, 10);
            var orderBy = "fullname";
            var isDescending = false;

            // Act
            var spec = UserSpecificationHelper.CreateSpecification(paginationFilter, orderBy, isDescending);

            // Assert
            Assert.NotNull(spec);
            Assert.Equal(paginationFilter.PageIndex - 1, spec.Skip);
        }

        [Fact]
        public void CreateSpecification_ValidInputsDescendingOrder_ReturnsSpecificationWithDescendingOrder()
        {
            // Arrange
            var paginationFilter = new PaginationFilter(1, 10);
            var orderBy = "lastname";
            var isDescending = true;

            // Act
            var spec = UserSpecificationHelper.CreateSpecification(paginationFilter, orderBy, isDescending);

            // Assert
            Assert.NotNull(spec);
            Assert.Equal(paginationFilter.PageIndex - 1, spec.Skip);
        }

        [Fact]
        public void CreateSpecification_NullOrderBy_ReturnsSpecificationWithDefaultOrder()
        {
            // Arrange
            var paginationFilter = new PaginationFilter(1, 10);
            string? orderBy = null;
            var isDescending = false;

            // Act
            var spec = UserSpecificationHelper.CreateSpecification(paginationFilter, orderBy, isDescending);

            // Assert
            Assert.NotNull(spec);
            Assert.Equal(paginationFilter.PageIndex - 1, spec.Skip);
        }

        [Fact]
        public void GetOrderByExpression_ValidOrderBy_ReturnsCorrectExpression()
        {
            // Arrange
            var orderBy = "fullname";

            // Act
            var expression = UserSpecificationHelper.GetOrderByExpression(orderBy);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<Expression<Func<User, object>>>(expression);
        }

        [Fact]
        public void GetOrderByExpression_InvalidOrderBy_ReturnsDefaultExpression()
        {
            // Arrange
            var orderBy = "invalidproperty";

            // Act
            var expression = UserSpecificationHelper.GetOrderByExpression(orderBy);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<Expression<Func<User, object>>>(expression);
        }

        [Fact]
        public void GetOrderByExpression_EmptyOrderBy_ReturnsDefaultExpression()
        {
            // Arrange
            var orderBy = "";

            // Act
            var expression = UserSpecificationHelper.GetOrderByExpression(orderBy);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<Expression<Func<User, object>>>(expression);
        }

        [Fact]
        public void GetUserByStaffCode_ValidStaffCode_ReturnsSpecification()
        {
            // Arrange
            var staffCode = "SC123";

            // Act
            var spec = UserSpecificationHelper.GetUserByStaffCode(staffCode);

            // Assert
            Assert.NotNull(spec);
        }
    }
}
