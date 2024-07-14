using System;
using System.Linq.Expressions;
using Xunit;
using AssetManagement.Application.Extensions;

namespace AssetManagement.Application.Tests.Extensions
{
    public class ExpressionExtensionTests
    {
        [Fact]
        public void And_CombinesTwoExpressions()
        {
            // Arrange
            Expression<Func<int, bool>> expr1 = num => num > 5;
            Expression<Func<int, bool>> expr2 = num => num < 10;

            // Act
            var combinedExpression = expr1.And(expr2);

            // Assert
            Assert.NotNull(combinedExpression);
            var compiledExpression = combinedExpression.Compile();
            Assert.True(compiledExpression(7));  // 7 is greater than 5 and less than 10
            Assert.False(compiledExpression(4)); // 4 is not greater than 5
            Assert.False(compiledExpression(11)); // 11 is not less than 10
        }

        [Fact]
        public void And_ReturnsCorrectExpressionForEdgeCases()
        {
            // Arrange
            Expression<Func<int, bool>> expr1 = num => num >= 0;
            Expression<Func<int, bool>> expr2 = num => num <= 0;

            // Act
            var combinedExpression = expr1.And(expr2);

            // Assert
            Assert.NotNull(combinedExpression);
            var compiledExpression = combinedExpression.Compile();
            Assert.True(compiledExpression(0));  // 0 is greater than or equal to 0 and less than or equal to 0
            Assert.False(compiledExpression(1)); // 1 is not less than or equal to 0
            Assert.False(compiledExpression(-1)); // -1 is not greater than or equal to 0
        }

        [Fact]
        public void And_HandlesNullExpressions()
        {
            // Arrange
            Expression<Func<int, bool>> expr1 = null;
            Expression<Func<int, bool>> expr2 = num => num < 10;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => expr1.And(expr2));
        }
    }
}
