using FluentResults;

namespace AiTelegramChannel.ServerHost.Imgur;

public interface IUnsplashClient
{
    Task<Result<string>> GetImageUrl(string query);
}