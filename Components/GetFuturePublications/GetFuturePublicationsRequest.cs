using AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;
using FluentResults;
using MediatR;

namespace AiTelegramChannel.ServerHost.Components.GetFuturePublications;

public class GetFuturePublicationsRequest : IRequest<Result<GetFuturePublicationsResponse>>
{
}