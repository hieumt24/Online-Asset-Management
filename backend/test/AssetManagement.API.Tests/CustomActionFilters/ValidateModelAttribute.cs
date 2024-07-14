using AssetManagement.API.CustomActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace AssetManagement.Tests
{
    public class ValidateModelAttributeTests
    {
        [Fact]
        public void OnActionExecuted_ModelStateIsInvalid_SetsResultToBadRequest()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                modelState
            );

            var context = new ActionExecutedContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Mock<Controller>().Object
            );

            context.ModelState.AddModelError("key", "error message");

            var filter = new ValidateModelAttribute();

            // Act
            filter.OnActionExecuted(context);

            // Assert
            Assert.IsType<BadRequestResult>(context.Result);
        }

        [Fact]
        public void OnActionExecuted_ModelStateIsValid_DoesNotSetResult()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                modelState
            );

            var context = new ActionExecutedContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Mock<Controller>().Object
            );

            var filter = new ValidateModelAttribute();

            // Act
            filter.OnActionExecuted(context);

            // Assert
            Assert.Null(context.Result);
        }
    }
}
