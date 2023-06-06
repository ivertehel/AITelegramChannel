using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;
using AutoMapper;

namespace AiTelegramChannel.ServerHost;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PublicationEntity, GetFuturePublicationsResponseModel>();
    }
}