using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;
using AiTelegramChannel.ServerHost.Extensions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTelegramChannel.ServerHost.Components.GetFuturePublications;

public class GetFuturePublicationsHandler : IRequestHandler<GetFuturePublicationsRequest, Result<GetFuturePublicationsResponse>>
{
    private readonly InMemoryContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetFuturePublicationsHandler> _logger;

    public GetFuturePublicationsHandler(InMemoryContext context, IMapper mapper, ILogger<GetFuturePublicationsHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<GetFuturePublicationsResponse>> Handle(GetFuturePublicationsRequest request, CancellationToken cancellationToken)
    {
        _logger.TraceEnter();

        var allPublications = await _context.Publications.OrderBy(p => p.CreatedOn).ToListAsync();

        var nextPublication = allPublications.OrderBy(p => p.CreatedOn).FirstOrDefault();

        var response = new GetFuturePublicationsResponse
        {
            Publications = _mapper.Map<IEnumerable<GetFuturePublicationsResponseModel>>(allPublications),
            NextPublication = _mapper.Map<GetFuturePublicationsResponseModel>(nextPublication)
        };

        return await _logger.TraceExit(Task.FromResult(response));
    }
}