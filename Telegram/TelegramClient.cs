using AiTelegramChannel.ServerHost.Extensions;
using AiTelegramChannel.ServerHost.Options;
using Microsoft.Extensions.Options;
using Telegram.Bots;
using Telegram.Bots.Requests;
using Telegram.Bots.Types;

namespace AiTelegramChannel.ServerHost.Telegram;

public class TelegramClient : ITelegramClient
{
    private readonly TelegramSettings _telegramSettings;
    private readonly IBotClient _botClient;
    private readonly ILogger<TelegramClient> _logger;

    public TelegramClient(
        IOptions<TelegramSettings> telegramSettings, 
        IBotClient botClient,
        ILogger<TelegramClient> logger)
    {
        _telegramSettings = telegramSettings?.Value ?? throw new ArgumentNullException();
        _botClient = botClient;
        _logger = logger;
    }

    public async Task PostMessageWithImage(string text, Uri imageUrl)
    {
        _logger.TraceEnter(argument: new { text, imageUrl });
        var request = new SendMediaGroup(_telegramSettings.ChatId, new List<IGroupableMedia>
        {
            new PhotoUrl(imageUrl)
            {
                Caption = text
            }
        })
        {
            DisableNotification = true
        };

        _logger.TraceExit(await _botClient.HandleAsync(request));
    }

    public async Task PostSimpleMessage(string text)
    {
        _logger.TraceEnter(argument: text);

        await _botClient.HandleAsync(new SendText(_telegramSettings.ChatId, text));

        _logger.TraceExit();
    }
}