using Calibre_net.Shared.Contracts;
using FluentValidation;

namespace Calibre_net.Shared.Validators;

public class CustomPageValidator : AbstractValidator<CustomPageDto>
{
    public CustomPageValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
    }

    public Func<object, string, Task<IEnumerable<string>>>? ValidateValue { get; set; }


}