using FluentValidation;
using TFortisDeviceManager.Helpers;
using TFortisDeviceManager.ViewModels;

namespace TFortisDeviceManager.Validators
{
    public class ManualAddingValidator : AbstractValidator<ManualAddingViewModel>
    {
        public ManualAddingValidator()
        {
            RuleFor(x => x.IpAddress).Must(x => IpAddressHelper.ValidateIPv4(x)).WithMessage(Properties.Resources.IpValidationError);
        }
    }
}
