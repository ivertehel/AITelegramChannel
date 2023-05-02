using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;
using AiTelegramChannel.ServerHost.Extensions;
using AutoMapper;
using FluentResults;
using MediatR;

namespace AiTelegramChannel.ServerHost.Components.GetFuturePublications;

public class GetFuturePublicationsHandler : IRequestHandler<GetFuturePublicationsRequest, Result<GetFuturePublicationsResponse>>
{
    private readonly PublicationsCache _publicationsCache;
    private readonly IMapper _mapper;
    private readonly ILogger<GetFuturePublicationsHandler> _logger;

    public GetFuturePublicationsHandler(PublicationsCache publicationsCache, IMapper mapper, ILogger<GetFuturePublicationsHandler> logger)
    {
        _publicationsCache = publicationsCache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<GetFuturePublicationsResponse>> Handle(GetFuturePublicationsRequest request, CancellationToken cancellationToken)
    {
        _logger.TraceEnter();

        var response = new GetFuturePublicationsResponse
        {
            Publications = _mapper.Map<IEnumerable<GetFuturePublicationsResponseModel>>(_publicationsCache.GetPublications()),
            NextPublication = _mapper.Map<GetFuturePublicationsResponseModel>(_publicationsCache.NextPublication)
        };

        return await _logger.TraceExit(Task.FromResult(response));
    }
}
