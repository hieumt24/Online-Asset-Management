using AssetManagement.Application.Common;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assignments;
using AssetManagement.Application.Models.DTOs.ReturnRequests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Reponses;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AutoMapper;
using Azure.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Services
{
    public class ReturnRequestService : IReturnRequestServiceAsync
    {
        private readonly IMapper _mapper;
        private readonly IReturnRequestRepositoriesAsync _returnRequestRepository;
        private readonly IUserRepositoriesAsync _userRepository;
        private readonly IAssignmentRepositoriesAsync _assignmentRepository;
        private readonly IValidator<AddReturnRequestDto> _addReturnRequestValidator;
        private readonly IUriService _uriService;
        private readonly IAssetRepositoriesAsync _assetRepository;
        private readonly IAssetServiceAsync _assetService;
        private readonly IAssetRepositoriesAsync _assetRepositoriesAsync;

        public ReturnRequestService(
            IReturnRequestRepositoriesAsync returnRequestRepository,
            IMapper mapper,
            IValidator<AddReturnRequestDto> addReturnRequestValidator,
            IUserRepositoriesAsync userRepository,
            IAssignmentRepositoriesAsync assignmentRepository,
            IUriService uriService,
            IAssetRepositoriesAsync assetRepository,
            IAssetServiceAsync assetService,
            IAssetRepositoriesAsync assetRepositoriesAsync
            )
        {
            _mapper = mapper;
            _returnRequestRepository = returnRequestRepository;
            _addReturnRequestValidator = addReturnRequestValidator;
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
            _uriService = uriService;
            _assetRepository = assetRepository;
            _assetService = assetService;
            _assetRepositoriesAsync = assetRepositoriesAsync;
        }

        public async Task<Response<ReturnRequestDto>> AddReturnRequestAsync(AddReturnRequestDto request)
        {
            var validatorRequest = await _addReturnRequestValidator.ValidateAsync(request);
            if (!validatorRequest.IsValid)
            {
                var errors = validatorRequest.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<ReturnRequestDto>
                {
                    Succeeded = false,
                    Errors = errors
                };
            }

            var existingAssignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
            if (existingAssignment == null)
            {
                return new Response<ReturnRequestDto> { Succeeded = false, Message = "The assignment no longer exists." };
            }

            var existingUserRequested = await _userRepository.GetByIdAsync(request.RequestedBy);
            if (existingUserRequested == null)
            {
                return new Response<ReturnRequestDto> { Succeeded = false, Message = "Requester not found." };
            }

            try
            {
                if (existingAssignment.State != EnumAssignmentState.Accepted) {
                     return new Response<ReturnRequestDto> { Succeeded = false, Message = "Cannot create a return request with this assignment." };
                }
                var newReturnRequest = _mapper.Map<ReturnRequest>(request);
                newReturnRequest.CreatedOn = DateTime.Now;
                newReturnRequest.CreatedBy = request.RequestedBy.ToString();
                newReturnRequest.LastModifiedOn = DateTime.Now;
                newReturnRequest.ReturnState = EnumReturnRequestState.WaitingForReturning;
                existingAssignment.State = EnumAssignmentState.WaitingForReturning;
                await _assignmentRepository.UpdateAsync(existingAssignment);
                var returnrequest = await _returnRequestRepository.AddAsync(newReturnRequest);

                var assetDto = _mapper.Map<ReturnRequestDto>(returnrequest);

                return new Response<ReturnRequestDto> { Succeeded = true, Message = "Create Return Request Successfully." };
            }
            catch (Exception ex)
            {
                return new Response<ReturnRequestDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<string>> CancelReturnRequestAsync(Guid returnRequestId)
        {
            try
            {
                var returnRequest = await _returnRequestRepository.GetByIdAsync(returnRequestId);
                if (returnRequest == null)
                {
                    return new Response<string> { Succeeded = false, Message = "Return Request not found." };
                }
                ///
                var existingAssignment = await _assignmentRepository.GetByIdAsync(returnRequest.AssignmentId);

                if (existingAssignment == null)
                {
                    return new Response<string>
                    {
                        Succeeded = false,
                        Message = "Assignment not found."
                    };
                }

                if (returnRequest.ReturnState == EnumReturnRequestState.WaitingForReturning)
                {
                    var cancelReturnRequest = await _returnRequestRepository.DeleteAsync(returnRequest.Id);
                    if (cancelReturnRequest == null)
                    {
                        return new Response<string> { Succeeded = false, Message = "Cancel Return Request failed." };
                    }
                    existingAssignment.State = EnumAssignmentState.Accepted;
                    existingAssignment.ReturnRequestId = null;
                    await _assignmentRepository.UpdateAsync(existingAssignment);
                }
                return new Response<string> { Succeeded = true, Message = "Cancel Return Request Successfully." };
            }
            catch (Exception ex)
            {
                return new Response<string> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public Task<Response<ReturnRequestDto>> EditReturnRequestAsync(EditReturnRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<List<ReturnRequestResponseDto>>> GetAllReturnRequestAsync(PaginationFilter pagination, string? search, EnumReturnRequestState? state, DateTime? returnedDateFrom, DateTime? returnedDateTo, EnumLocation location, string? orderBy, bool? isDescending, string? route)
        {
            if (returnedDateFrom > returnedDateTo)
            {
                return new PagedResponse<List<ReturnRequestResponseDto>> { Succeeded = false, Message = "Returned Date From need before Returned Date To" };
            }
            try
            {
                if (pagination == null)
                {
                    pagination = new PaginationFilter();
                }
                var filterReturnRequest = await _returnRequestRepository.FilterReturnRequestAsync(location, search, state, returnedDateFrom, returnedDateTo);

                var totalRecords = await filterReturnRequest.CountAsync();

                var specRequestReturn = ReturnRequestSpecificationHelper.CreateSpecReturnRequest(pagination, orderBy, isDescending);

                var returnRequests = await SpecificationEvaluator<ReturnRequest>.GetQuery(filterReturnRequest, specRequestReturn).ToListAsync();

                var returnRequestResponseDtos = _mapper.Map<List<ReturnRequestResponseDto>>(returnRequests);

                var pagedResponsed = PaginationHelper.CreatePagedReponse(returnRequestResponseDtos, pagination, totalRecords, _uriService, route);
                return pagedResponsed;
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<ReturnRequestResponseDto>> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public Task<Response<ReturnRequestDto>> GetReturnRequestByIdAsync(Guid assignmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<ReturnRequestDto>> ChangeAssignmentStateAsync(ChangeStateReturnRequestDto request)
        {
            var returnRequest = await _returnRequestRepository.GetByIdAsync(request.ReturnRequestId);

            if (returnRequest == null)
            {
                return new Response<ReturnRequestDto>
                {
                    Succeeded = false,
                    Message = "The return request is already cancelled."
                };
            }
            if (returnRequest.ReturnState == EnumReturnRequestState.Completed)
            {
                return new Response<ReturnRequestDto> { Succeeded = false, Message = "This return request is already accepted." };
            }
            returnRequest.ReturnState = request.NewState;

            

            if (request.NewState == EnumReturnRequestState.Completed)
            {
                var assignment = await _assignmentRepository.GetByIdAsync(returnRequest.AssignmentId);

                if (assignment == null)
                {
                    return new Response<ReturnRequestDto>
                    {
                        Succeeded = false,
                        Message = "Assignment not found."
                    };
                }

              
                var assetResponse = await _assetRepositoriesAsync.GetByIdAsync(assignment.AssetId);

                returnRequest.ReturnedDate = DateTime.Now;
                returnRequest.AcceptedBy = request.AcceptedBy;
                assetResponse.State = AssetStateType.Available;
                assignment.State = EnumAssignmentState.Returned;

                try
                {
                    await _assignmentRepository.UpdateAsync(assignment);
                    await _assetRepository.UpdateAsync(assetResponse);
                }
                catch (Exception ex)
                {
                    return new Response<ReturnRequestDto>
                    {
                        Succeeded = false,
                        Message = "An error occurred while updating assignment or asset state.",
                        Errors = new List<string> { ex.Message }
                    };
                }
            }

            try
            {
                await _returnRequestRepository.UpdateAsync(returnRequest);
                var assignmentDto = _mapper.Map<ReturnRequestDto>(returnRequest);
                return new Response<ReturnRequestDto>
                {
                    Succeeded = true,
                    Message = "Return request state changed successfully.",
                };
            }
            catch (Exception ex)
            {
                return new Response<ReturnRequestDto>
                {
                    Succeeded = false,
                    Message = "An error occurred while changing the assignment state.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}