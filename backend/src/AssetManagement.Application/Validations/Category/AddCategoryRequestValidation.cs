using AssetManagement.Application.Models.DTOs.Category.Requests;
using FluentValidation;
using System.ComponentModel;

namespace AssetManagement.Application.Validations.Category
{
    public class AddCategoryRequestValidation : AbstractValidator<AddCategoryRequestDto>
    {
        public AddCategoryRequestValidation()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name cannot be blank")
                .Length(2, 50).WithMessage("The category name length should be 2 - 50 characters")
                .Matches(@"^(?=.*[a-zA-Z])[a-zA-Z0-9!@#$%^&*(),.?':{ }|<> ~`+=\-_ /\\[\] ]*$").WithMessage("The category name must contain letters");

            RuleFor(x => x.Prefix)
                .NotEmpty().WithMessage("Prefix cannot be blank")
                .Length(2, 5).WithMessage("Prefix length should be 2 - 5 characters")
                .Matches(@"[a-zA-Z]").WithMessage("The prefix contains only letters");
        }
    }
}
