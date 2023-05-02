using AiTelegramChannel.ServerHost.BackgroundJobs;
using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Imgur;
using AiTelegramChannel.ServerHost.OpenAi;
using AiTelegramChannel.ServerHost.Telegram;
using OpenAI.GPT3.Extensions;
using Telegram.Bots;

namespace AiTelegramChannel.ServerHost.Extensions;

public static class ServiceRegistrationExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IChatGptClient, ChatGptClient>();
        services.AddTransient<IUnsplashClient, UnsplashClient>();
        services.AddTransient<ITelegramClient, TelegramClient>();
        services.AddOpenAIService(settings => { settings.ApiKey = configuration.GetValue<string>("OpenAi:ApiKey"); });
        services.AddBotClient(configuration.GetValue<string>("TelegramSettings:Token"));

        services.AddHostedService<PostsGeneratorBackgroundJob>();
        services.AddHostedService<PostPublisherBackgroundJob>();

        services.AddSingleton<PublicationsCache>();
    }
}