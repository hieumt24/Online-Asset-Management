using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assignments.Requests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.API.Controllers
{
    [Route("api/v1/returnRequests")]
    [ApiController]
    public class ReturnRequestController : ControllerBase
    {
        private readonly IReturnRequestServiceAsync _returnRequestServicesAsync;

        public ReturnRequestController(IReturnRequestServiceAsync returnRequestServicesAsync)
        {
            _returnRequestServicesAsync = returnRequestServicesAsync;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddReturnRequestDto request)
        {
            var result = await _returnRequestServicesAsync.AddReturnRequestAsync(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("filter-return-requests")]
        public async Task<IActionResult> GetAll([FromBody] ReturnRequestFilter returnRequestFilter)
        {
            string route = Request.Path.Value;
            var result = await _returnRequestServicesAsync.GetAllReturnRequestAsync(returnRequestFilter.pagination, returnRequestFilter.search, returnRequestFilter.returnState, returnRequestFilter.returnedDateFrom, returnRequestFilter.returnedDateTo, returnRequestFilter.location, returnRequestFilter.orderBy, returnRequestFilter.isDescending, route);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> CancelReturnRequest(Guid returnRequestId)
        {
            var result = await _returnRequestServicesAsync.CancelReturnRequestAsync(returnRequestId);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeReturnRequestStatus(ChangeStateReturnRequestDto request)
        {
            var result = await _returnRequestServicesAsync.ChangeAssignmentStateAsync(request);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
