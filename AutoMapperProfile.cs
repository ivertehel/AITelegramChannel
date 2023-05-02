using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;
using AutoMapper;

namespace AiTelegramChannel.ServerHost;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PublicationModel, GetFuturePublicationsResponseModel>()
            .ForMember(dst => dst.PublishTime, opt => opt.MapFrom(src => src.PublishAt.HasValue ? TimeOnly.FromDateTime(src.PublishAt.Value) : (TimeOnly?)null));
    }
}