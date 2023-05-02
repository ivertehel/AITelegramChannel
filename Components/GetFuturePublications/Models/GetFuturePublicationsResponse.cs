namespace AiTelegramChannel.ServerHost.Components.GetFuturePublications.Models;

public class GetFuturePublicationsResponse
{
    public GetFuturePublicationsResponseModel NextPublication { get; set; }

    public IEnumerable<GetFuturePublicationsResponseModel> Publications { get; set; }

}

public class GetFuturePublicationsResponseModel
{
    public TimeOnly? PublishTime { get; set; }

    public string Content { get; set; }

    public string Image { get; set; }
}