using AiTelegramChannel.ServerHost.Options;
using AiTelegramChannel.ServerHost.Unsplash.Models;
using FluentResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace AiTelegramChannel.ServerHost.Imgur;

public class UnsplashClient : IUnsplashClient
{
    private readonly UnsplashSettings _unsplashSettings;

    public UnsplashClient(IOptions<UnsplashSettings> imgurSettings)
    {
        _unsplashSettings = imgurSettings?.Value ?? throw new ArgumentNullException(nameof(imgurSettings));
    }

    public async Task<Result<string>> GetImageUrl(string query)
    {
        var options = new RestClientOptions(_unsplashSettings.BaseUrl)
        {
            MaxTimeout = -1,
        };

        using var client = new RestClient(options);

        var request = new RestRequest($"/search/photos?query={query}&page=1&per_page=1", Method.Get);
        request.AddHeader("Authorization", $"Client-ID {_unsplashSettings.ClientId}");

        var response = await client.ExecuteAsync<UnsplashResponse>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data.Results.First().Urls.Regular;
        }

        return Result.Fail($"{nameof(UnsplashClient)} returned unsuccessful response. {JsonConvert.SerializeObject(response)}");
    }
}