using AiTelegramChannel.ServerHost.Options;

namespace AiTelegramChannel.ServerHost.Extensions;

public static class ServiceCollectionConfigurationExtensions
{
    public static void RegisterOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<PostsGeneratorBackgroundJobSettings>(configuration.GetSection(key: nameof(PostsGeneratorBackgroundJobSettings)));
        serviceCollection.Configure<UnsplashSettings>(configuration.GetSection(key: nameof(UnsplashSettings)));
        serviceCollection.Configure<TelegramSettings>(configuration.GetSection(key: nameof(TelegramSettings)));
        serviceCollection.Configure<ApiSettings>(configuration.GetSection(key: nameof(ApiSettings)));
    }
}