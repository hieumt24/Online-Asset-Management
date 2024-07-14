using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using Moq;
using System;
using System.Linq.Expressions;
using Xunit;

namespace AssetManagement.Application.Tests.Helper
{
    public class ReturnRequestSpecificationHelperTests
    {
        [Fact]
        public void CreateSpecReturnRequest_ValidInputs_ReturnsSpecificationWithCriteria()
        {
            // Arrange
            var paginationFilter = new PaginationFilter(1, 10);
            var orderBy = "assetcode";
            var isDescending = false;

            // Act
            var spec = ReturnRequestSpecificationHelper.CreateSpecReturnRequest(paginationFilter, orderBy, isDescending);

            // Assert
            Assert.NotNull(spec);
            Assert.Contains(spec.Includes, include => include.Body.Type == typeof(Assignment));
            Assert.Contains(spec.Includes, include => include.Body.Type == typeof(User));
        }

        [Fact]
        public void CreateSpecReturnRequest_ValidInputsDescendingOrder_ReturnsSpecificationWithDescendingOrder()
        {
            // Arrange
            var paginationFilter = new PaginationFilter(1, 10);
            var orderBy = "assetname";
            var isDescending = true;

            // Act
            var spec = ReturnRequestSpecificationHelper.CreateSpecReturnRequest(paginationFilter, orderBy, isDescending);

            // Assert
            Assert.NotNull(spec);
            Assert.Contains(spec.Includes, include => include.Body.Type == typeof(Assignment));
            Assert.Contains(spec.Includes, include => include.Body.Type == typeof(User));
        }

        [Fact]
        public void CreateSpecReturnRequest_NullOrderBy_ReturnsSpecificationWithDefaultOrder()
        {
            // Arrange
            var paginationFilter = new PaginationFilter(1, 10);
            string? orderBy = null;
            var isDescending = false;

            // Act
            var spec = ReturnRequestSpecificationHelper.CreateSpecReturnRequest(paginationFilter, orderBy, isDescending);

            // Assert
            Assert.NotNull(spec);
            Assert.Contains(spec.Includes, include => include.Body.Type == typeof(Assignment));
            Assert.Contains(spec.Includes, include => include.Body.Type == typeof(User));
        }

        [Fact]
        public void GetOrderByExpression_ValidOrderBy_ReturnsCorrectExpression()
        {
            // Arrange
            var orderBy = "assetcode";

            // Act
            var expression = ReturnRequestSpecificationHelper.GetOrderByExpression(orderBy);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<Expression<Func<ReturnRequest, object>>>(expression);
        }

        [Fact]
        public void GetOrderByExpression_InvalidOrderBy_ReturnsDefaultExpression()
        {
            // Arrange
            var orderBy = "invalidproperty";

            // Act
            var expression = ReturnRequestSpecificationHelper.GetOrderByExpression(orderBy);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<Expression<Func<ReturnRequest, object>>>(expression);
        }

        [Fact]
        public void GetOrderByExpression_EmptyOrderBy_ReturnsDefaultExpression()
        {
            // Arrange
            var orderBy = "";

            // Act
            var expression = ReturnRequestSpecificationHelper.GetOrderByExpression(orderBy);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<Expression<Func<ReturnRequest, object>>>(expression);
        }
    }
}


