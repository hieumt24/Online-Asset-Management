using AssetManagement.API.CustomActionFilters;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.API.Controllers
{
    [Route("api/v1/assets")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        public readonly IAssetServiceAsync _assetService;

        public AssetController(IAssetServiceAsync assetService)
        {
            _assetService = assetService;
        }

        [HttpPost]
        [Route("filter-assets")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsset([FromBody] AssetFilter assetFilter)
        {
            string route = Request.Path.Value;
            var response = await _assetService.GetAllAseets(assetFilter.pagination, assetFilter.search, assetFilter.categoryId, assetFilter.assetStateType, assetFilter.adminLocation, assetFilter.orderBy, assetFilter.isDescending, route);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AddAssetRequestDto request)
        {
            var response = await _assetService.AddAssetAsync(request);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromQuery] Guid id, [FromBody] EditAssetRequestDto request)
        {
            var response = await _assetService.EditAssetAsync(id, request);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            var result = await _assetService.DeleteAssetAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAssetById(Guid id)
        {
            var result = await _assetService.GetAssetByIdAsync(id);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet]
        [Route("assetCode/{assetCode}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetUserByAssetCode(string assetCode)
        {
            var response = await _assetService.GetAssetByAssetCode(assetCode);
            if (!response.Succeeded)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("isValidDeleteAsset/{assetId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IsValidDeleteAsset(Guid assetId)
        {
            var response = await _assetService.IsValidDeleteAsset(assetId);

            if (!response.Succeeded)
            {
               
                return BadRequest(response);
            }
            
            return Ok(response);
        }
    }
}