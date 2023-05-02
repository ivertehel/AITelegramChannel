namespace AiTelegramChannel.ServerHost.Options;

public class PostsGeneratorBackgroundJobSettings
{
    public bool Enabled { get; set; }

    public string Message { get; set; }

    public int DelayInMinutesFrom { get; set; }

    public int DelayInMinutesTo { get; set; }

    public int DelayBetweenExecutions { get; set; }

    public bool GenerateImages { get; set; }
}