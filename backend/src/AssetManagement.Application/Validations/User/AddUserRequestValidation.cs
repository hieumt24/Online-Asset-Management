using AssetManagement.Application.Models.DTOs.Users.Requests;
using FluentValidation;
using AssetManagement.Domain.Enums;
using System;

namespace AssetManagement.Application.Validations.User
{
    public class AddUserRequestValidation : AbstractValidator<AddUserRequestDto>
    {
        public AddUserRequestValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Matches("^[a-zA-Z]+$").WithMessage("First name must only contain alphabetic characters.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Last name must only contain alphabetic characters and spaces.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters long.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(BeAValidDate).WithMessage("Date of birth is not a valid date.")
                .Must(dob => IsOlderThan(dob, 18)).WithMessage("User must be at least 18 years old.");

            RuleFor(x => x.JoinedDate)
                .NotEmpty().WithMessage("Joined Date is required.")
                .Must(BeAValidDate).WithMessage("Invalid joined date.")
                .Must(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday).WithMessage("Joined Date can not be Saturday or Sunday.")
                .GreaterThan(x => x.DateOfBirth).WithMessage("Joined Date must be after Date of Birth.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Please enter Gender")
                .NotNull().WithMessage("Please enter Gender")
                .Must(gender => Enum.IsDefined(typeof(GenderEnum), gender)).WithMessage("Invalid Gender");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Please enter Role")
                .NotNull().WithMessage("Please enter Role")
                .Must(roleId => Enum.IsDefined(typeof(RoleType), roleId)).WithMessage("Invalid Role");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Please enter Location")
                .NotNull().WithMessage("Please enter Location")
                .Must(role => Enum.IsDefined(typeof(EnumLocation), role)).WithMessage("Invalid Location");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default;
        }

        private bool IsOlderThan(DateTime dob, int age)
        {
            var today = DateTime.Today;
            var calculatedAge = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-calculatedAge)) calculatedAge--;
            return calculatedAge >= age;
        }
    }
}