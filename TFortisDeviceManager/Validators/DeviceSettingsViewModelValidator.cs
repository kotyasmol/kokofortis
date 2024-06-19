using FluentValidation;
using TFortisDeviceManager.Helpers;
using TFortisDeviceManager.ViewModels;

namespace TFortisDeviceManager.Validators;

public class DeviceSettingsViewModelValidator : AbstractValidator<DeviceSettingsViewModel>
{
    public DeviceSettingsViewModelValidator()
    {
        RuleFor(x => x.IpAddress).Must(x => IpAddressHelper.ValidateIPv4(x, false)).WithMessage(Properties.Resources.IpValidationError);
        RuleFor(x => x.NetworkMask).Must(x => IpAddressHelper.ValidateIPv4(x, false)).WithMessage(Properties.Resources.NetworkMaskValidationError);
        RuleFor(x => x.Gateway).Must(x => IpAddressHelper.ValidateIPv4(x, true)).WithMessage(Properties.Resources.GatewayValidationError);
    }
}