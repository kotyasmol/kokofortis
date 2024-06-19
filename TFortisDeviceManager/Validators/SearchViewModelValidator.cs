using FluentValidation;
using TFortisDeviceManager.Helpers;
using TFortisDeviceManager.ViewModels;

namespace TFortisDeviceManager.Validators
{
    public class SearchViewModelValidator : AbstractValidator<SearchViewModel>
    {
        public SearchViewModelValidator()
        {
            RuleFor(x => x.FromIpAddress).Must(x => IpAddressHelper.ValidateIPv4(x)).WithMessage(Properties.Resources.IpValidationError);            
            RuleFor(x => x.ToIpAddress).Must(x => IpAddressHelper.ValidateIPv4(x)).WithMessage(Properties.Resources.IpValidationError);
            //RuleFor(x => x.SntpServer).Must(x => IpAddressHelper.ValidateIPv4(x)).WithMessage(Properties.Resources.IpValidationError);
        }
    }
}
