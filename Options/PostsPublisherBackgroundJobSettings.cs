namespace AiTelegramChannel.ServerHost.Options;

public class PostsPublisherBackgroundJobSettings
{
    public bool Enabled { get; set; }

    public int DelayInMinutesFrom { get; set; }

    public int DelayInMinutesTo { get; set; }
}
