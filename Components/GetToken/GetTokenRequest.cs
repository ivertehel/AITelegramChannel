using FluentResults;
using MediatR;

namespace AiTelegramChannel.ServerHost.Components.GetToken;

public class GetTokenRequest : IRequest<Result<string>>
{
    public string ApiKey { get; set; }
}