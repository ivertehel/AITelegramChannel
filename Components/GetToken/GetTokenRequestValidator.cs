using FluentValidation;

namespace AiTelegramChannel.ServerHost.Components.GetToken;

public class GetTokenRequestValidator : AbstractValidator<GetTokenRequest>
{
    public GetTokenRequestValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty();
    }
}