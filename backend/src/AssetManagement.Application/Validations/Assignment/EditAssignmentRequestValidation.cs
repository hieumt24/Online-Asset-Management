using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.Validations.Assignment
{
    public class EditAssignmentRequestValidation : AbstractValidator<EditAssignmentRequestDto>
    {
        public EditAssignmentRequestValidation()
        {
            RuleFor(x => x.AssignedIdTo)
              .NotEmpty().WithMessage("AssignedIdTo cannot be blank.")
              .Must(id => id != Guid.Empty).WithMessage("AssignedIdTo must be a valid GUID.");

            RuleFor(x => x.AssignedIdBy)
                .NotEmpty().WithMessage("AssignedIdBy cannot be blank.")
                .Must(id => id != Guid.Empty).WithMessage("AssignedIdBy must be a valid GUID.");

            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId cannot be blank.")
                .Must(id => id != Guid.Empty).WithMessage("AssetId must be a valid GUID.");
            //> 2000 
            RuleFor(x => x.AssignedDate)
                .NotEmpty().WithMessage("AssignedDate cannot be blank.")
                .GreaterThanOrEqualTo(new DateTime(2000, 1, 1)).WithMessage("Please select valid Assigned Date ");

            RuleFor(x => x.Note)
                .Length(0, 256).WithMessage("Note must not be longer than 256 characters.");
        } 
    }
}
