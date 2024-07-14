using AssetManagement.Application.Models.DTOs.Assets;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assets.Responses;
using AssetManagement.Application.Models.DTOs.Assignments;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Assignments.Response;
using AssetManagement.Application.Models.DTOs.Category;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Reponses;
using AssetManagement.Application.Models.DTOs.ReturnRequests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Domain.Entites;
using AutoMapper;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;

namespace AssetManagement.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserDto, UpdateUserRequestDto>().ReverseMap();
            CreateMap<AddUserRequestDto, User>().ReverseMap();

            //Asset Mapping
            CreateMap<AddAssetRequestDto, Asset>().ReverseMap();
            CreateMap<EditAssetRequestDto, Asset>().ReverseMap();
            CreateMap<Asset, AssetDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<Asset, AssetResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();

            //Category Mapping
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<AddCategoryRequestDto, Category>().ReverseMap();
            CreateMap<CategoryDto, UpdateCategoryRequestDto>().ReverseMap();

            //Assignment Mapping
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<AddAssignmentRequestDto, Assignment>().ReverseMap();
            CreateMap<EditAssignmentRequestDto, AssignmentDto>().ReverseMap();
            CreateMap<Assignment, AssignmentResponseDto>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(opt => opt.Asset.AssetName))
                .ForMember(dest => dest.Specification, opt => opt.MapFrom(opt => opt.Asset.Specification))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(opt => opt.AssignedTo.Username))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(opt => opt.AssignedBy.Username))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(opt => opt.CreatedOn))
                .ForMember(dest => dest.LastModifiedOn, opt => opt.MapFrom(opt => opt.LastModifiedOn))
                .ReverseMap()
                ;

            CreateMap<ReturnRequest, ReturnRequestDto>().ReverseMap();
            CreateMap<AddReturnRequestDto, ReturnRequest>().ReverseMap();

            CreateMap<ReturnRequest, ReturnRequestResponseDto>()
                 .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Assignment.Asset.AssetCode))
                 .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Assignment.Asset.AssetName))
                 .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.RequestedUser.Username))
                 .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.Assignment.AssignedDate))
                 .ForMember(dest => dest.AcceptedBy, opt => opt.MapFrom(src => src.AcceptedUser.Username))
                 .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate))
                 .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ReturnState))
                 .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                 .ReverseMap();
        }
    }
}