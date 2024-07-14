using AssetManagement.Application.Filter;

namespace AssetManagement.Application.Interfaces.Services
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter filter, string route);
    }
}