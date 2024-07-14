using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.API.Controllers;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Reports.Responses;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;
using AssetManagement.Application.Filter;
using Microsoft.AspNetCore.Http;
using AssetManagement.Application.Models.Filters;

namespace AssetManagement.API.Tests.Controllers
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportServices> _mockReportServices;
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _mockReportServices = new Mock<IReportServices>();
            _controller = new ReportController(_mockReportServices.Object);
        }

        // Helper method to create DefaultHttpContext
        private DefaultHttpContext CreateHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/v1/reports";
            return httpContext;
        }

        [Fact]
        public async Task GetReport_ReturnsOkResult_WhenReportIsFetchedSuccessfully()
        {
            // Arrange
            var reportFilter = new ReportFIlter
            {
                Location = EnumLocation.HaNoi,
                Pagination = new PaginationFilter(),
                OrderBy = "CategoryName",
                IsDescending = false
            };
            var route = "/api/v1/reports";
            var response = new PagedResponse<List<ReportResponseDto>> { Succeeded = true, Data = new List<ReportResponseDto>() };

            _mockReportServices.Setup(service => service.GetReportPaginationAsync(reportFilter.Location, reportFilter.Pagination, reportFilter.search, reportFilter.OrderBy, reportFilter.IsDescending, route))
                               .ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetReport(reportFilter) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetReport_ReturnsBadRequest_WhenFetchingReportFails()
        {
            // Arrange
            var reportFilter = new ReportFIlter
            {
                Location = EnumLocation.HaNoi,
                Pagination = new PaginationFilter(),
                OrderBy = "CategoryName",
                IsDescending = false
            };
            var route = "/api/v1/reports";
            var response = new PagedResponse<List<ReportResponseDto>> { Succeeded = false };

            _mockReportServices.Setup(service => service.GetReportPaginationAsync(reportFilter.Location, reportFilter.Pagination, reportFilter.search ,  reportFilter.OrderBy, reportFilter.IsDescending, route))
                               .ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetReport(reportFilter) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task ExportReportToExcel_ReturnsFileResult_WhenReportIsExportedSuccessfully()
        {
            // Arrange
            var location = EnumLocation.HaNoi;
            var fileContents = new byte[] { 0x01, 0x02, 0x03 };

            _mockReportServices.Setup(service => service.ExportReportToExcelAsync(location)).ReturnsAsync(fileContents);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.ExportReportToExcel(location) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.ContentType);
            Assert.Equal("Report.xlsx", result.FileDownloadName);
            Assert.Equal(fileContents, result.FileContents);
        }


    }
}