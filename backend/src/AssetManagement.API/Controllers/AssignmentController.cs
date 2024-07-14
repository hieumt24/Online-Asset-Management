using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Assignments.Requests;
using AssetManagement.Application.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.API.Controllers
{
    [Route("api/v1/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentServicesAsync _assignmentServicesAsync;

        public AssignmentController(IAssignmentServicesAsync assignmentServicesAsync)
        {
            _assignmentServicesAsync = assignmentServicesAsync;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAssignment([FromBody] AddAssignmentRequestDto request)
        {
            var result = await _assignmentServicesAsync.AddAssignmentAsync(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("filter-assignments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FilterAssignment([FromBody] AssignmentFilter filter)
        {
            string route = Request.Path.Value;
            var response = await _assignmentServicesAsync.GetAllAssignmentsAsync(filter.pagination, filter.search, filter.assignmentState, filter.dateFrom, filter.dateTo, filter.adminLocation, filter.orderBy, filter.isDescending, route);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("filter-user-assigned")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAssignmentsForUser([FromBody] FilterAssignmentForUser filterAssignmentForUser)
        {
            string route = Request.Path.Value;
            var result = await _assignmentServicesAsync.GetAssignmentsOfUser(filterAssignmentForUser.pagination, filterAssignmentForUser.userId, filterAssignmentForUser.search, filterAssignmentForUser.assignmentState, filterAssignmentForUser.dateFrom, filterAssignmentForUser.dateTo, filterAssignmentForUser.orderBy, filterAssignmentForUser.isDescending, route);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("filter-asset-assigned-history")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAssetAssignHistory([FromBody] FilterAssetAssignHistory filterAssignmentForUser)
        {
            string route = Request.Path.Value;
            var result = await _assignmentServicesAsync.GetAssetAssign(filterAssignmentForUser.pagination, filterAssignmentForUser.assetId, filterAssignmentForUser.search, filterAssignmentForUser.assignmentState, filterAssignmentForUser.dateFrom, filterAssignmentForUser.dateTo, filterAssignmentForUser.orderBy, filterAssignmentForUser.isDescending, route);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAssignmentById(Guid assignmentId)
        {
            var result = await _assignmentServicesAsync.GetAssignmentByIdAsync(assignmentId);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeAssignmentStatus(ChangeStateAssignmentDto request)
        {
            var result = await _assignmentServicesAsync.ChangeAssignmentStateAsync(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut]
        [Route("edit-assignment/{assignmentId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAssignment([FromBody] EditAssignmentRequestDto request, Guid assignmentId)
        {
            var result = await _assignmentServicesAsync.EditAssignmentAsync(request, assignmentId);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("{assignmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
        {
            var result = await _assignmentServicesAsync.DeleteAssignmentAsync(assignmentId);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
