using calibre_net.Client.Models;
using calibre_net.Components.Account.Pages;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace calibre_net.Validators;
    public class TokenRequestValidator : AbstractValidator<SignInModel>
    {
        public TokenRequestValidator(IStringLocalizer<Login> localizer)
        {
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email is required"])
                .EmailAddress().WithMessage(x => localizer["Email is not correct"]);
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Password is required!"]);
        }
    }