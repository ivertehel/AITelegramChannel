namespace AiTelegramChannel.ServerHost.OpenAi;

public interface IChatGptClient
{
    Task<string> SendMessage(string message);
}