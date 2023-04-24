using FluentResults;

namespace AiTelegramChannel.ServerHost.Imgur;

public interface IUnsplashClient
{
    Task<Result<string>> GetFirstImageUrl(string keyword);
}