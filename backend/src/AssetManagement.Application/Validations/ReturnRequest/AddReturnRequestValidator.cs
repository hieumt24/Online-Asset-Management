using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.Validations.ReturnRequest
{
    public class AddReturnRequestValidator: AbstractValidator<AddReturnRequestDto>
    {
        public AddReturnRequestValidator() 
        {
            RuleFor(x => x.AssignmentId)
              .NotEmpty().WithMessage("AssignmentId can not be blank.")
              .Must(id => id != Guid.Empty).WithMessage("AssignmentId must be a valid GUID.");

            //RuleFor(x => x.RequestedBy)
            //    .NotEmpty().WithMessage("RequestedBy can not be blank.")
            //    .Must(id => id != Guid.Empty).WithMessage("RequestedBy must be a valid GUID.");
        }
    }
}
