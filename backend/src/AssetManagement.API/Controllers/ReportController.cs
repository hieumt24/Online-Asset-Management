using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.API.Controllers
{
    [Route("api/v1/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportServices _reportServices;

        public ReportController(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReport([FromBody] ReportFIlter reportFIlter)
        {
            string route = Request.Path.Value;
            var result = await _reportServices.GetReportPaginationAsync(reportFIlter.Location, reportFIlter.Pagination, reportFIlter.search , reportFIlter.OrderBy, reportFIlter.IsDescending, route);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("export")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportReportToExcel([FromQuery] EnumLocation location)
        {
            var fileContents = await _reportServices.ExportReportToExcelAsync(location);
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }
    }
}