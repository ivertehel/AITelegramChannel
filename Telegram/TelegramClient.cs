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

    public TelegramClient(
        IOptions<TelegramSettings> telegramSettings, 
        IBotClient botClient)
    {
        _telegramSettings = telegramSettings?.Value ?? throw new ArgumentNullException();
        _botClient = botClient;
    }

    public async Task PostMessageWithImage(string text, Uri imageUrl)
    {
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

        await _botClient.HandleAsync(request);
    }

    public async Task PostSimpleMessage(string text)
    {
        await _botClient.HandleAsync(new SendText(_telegramSettings.ChatId, text));
    }
}