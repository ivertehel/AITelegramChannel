using System.Collections.Concurrent;

namespace AiTelegramChannel.ServerHost.Cache;

public class PublicationsCache
{
    private readonly ConcurrentQueue<PublicationModel> Publications = new ConcurrentQueue<PublicationModel>();

    public PublicationModel? NextPublication { get; private set; }

    public IEnumerable<PublicationModel> GetPublications() => Publications.ToList();

    public void AddPublication(PublicationModel publication, TimeSpan delay)
    {
        if (NextPublication == null)
        {
            NextPublication = new PublicationModel
            {
                Content = publication.Content,
                Image = publication.Image,
                Delay = delay,
                PublishAt = DateTime.Now.Add(delay)
            };

            return;
        }

        Publications.Enqueue(publication);
    }

    public void Publish()
    {
        if (Publications.TryDequeue(out var publication))
        {
            NextPublication = publication;
            NextPublication.PublishAt = DateTime.Now.Add(publication.Delay);
        }
    }
}

public class PublicationModel
{
    public string Content { get; set; }

    public string Image { get; set; }

    public TimeSpan Delay { get; set; }

    public DateTime? PublishAt { get; set; }
}