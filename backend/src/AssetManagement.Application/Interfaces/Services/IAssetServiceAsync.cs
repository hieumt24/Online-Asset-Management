using AssetManagement.Application.Filter;
using AssetManagement.Application.Models.DTOs.Assets;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assets.Responses;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Services
{
    public interface IAssetServiceAsync
    {
        Task<Response<AssetDto>> AddAssetAsync(AddAssetRequestDto request);

        Task<PagedResponse<List<AssetResponseDto>>> GetAllAseets(PaginationFilter pagination, string? search, Guid? categoryId, ICollection<AssetStateType?>? assetStateType, EnumLocation adminLocation, string? orderBy, bool? isDescending, string? route);

        Task<Response<AssetDto>> GetAssetByIdAsync(Guid assetId);

        Task<Response<AssetDto>> EditAssetAsync(Guid assetId, EditAssetRequestDto request);

        Task<Response<AssetDto>> DeleteAssetAsync(Guid assetId);

        Task<Response<AssetDto>> GetAssetByAssetCode(string assetCode);

        Task<Response<bool>> IsValidDeleteAsset(Guid assetId);
    }
}