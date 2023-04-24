using FluentResults;

namespace AiTelegramChannel.ServerHost.Imgur;

public interface IUnsplashClient
{
    Task<Result<string>> GetRandomImageUrl(string keyword);
}