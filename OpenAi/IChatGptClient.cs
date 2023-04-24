using FluentResults;

namespace AiTelegramChannel.ServerHost.OpenAi;

public interface IChatGptClient
{
    Task<Result<string>> SendMessage(string message);
}