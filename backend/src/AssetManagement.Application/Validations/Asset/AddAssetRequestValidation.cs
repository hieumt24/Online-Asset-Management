using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Domain.Enums;
using FluentValidation;

namespace AssetManagement.Application.Validations.Asset
{
    public class AddAssetRequestValidation : AbstractValidator<AddAssetRequestDto>
    {
        public AddAssetRequestValidation()
        {
            RuleFor(x => x.AssetName)
                .NotEmpty().WithMessage("Asset name cannot be blank")
                .Length(2, 50).WithMessage("The asset name length should be 2 - 50 characters")
                .Matches(@"[a-zA-Z]").WithMessage("The asset name must contain letters");

            RuleFor(x => x.Specification)
                .NotEmpty().WithMessage("Specification cannot be blank")
                .Length(2, 100).WithMessage("The specification length should be 2 - 50 characters")
                .Matches(@"[a-zA-Z]").WithMessage("The specification must contain letters");

            RuleFor(x => x.AssetLocation)
                .NotEmpty().WithMessage("Location cannot be blank")
                .NotNull().WithMessage("Location cannot be blank")
                .Must(role => Enum.IsDefined(typeof(EnumLocation), role)).WithMessage("Invalid Location");

            RuleFor(x => x.State)
               .NotEmpty().WithMessage("Please enter Location")
               .Must(location => location == AssetStateType.Available || location == AssetStateType.NotAvailable).WithMessage("Invalid State, can only choose available or not available");

            RuleFor(x => x.InstalledDate)
                .NotEmpty().WithMessage("Installed Date cannot be blank")
                .Must(BeAValidDate).WithMessage("Please select a valid Installed Date")
                .GreaterThanOrEqualTo(new DateTime(2000, 1, 1)).WithMessage("Installed Date must be from the year 2000 or later.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Installed Date cannot be a future date.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default;
        }
    }
}
 