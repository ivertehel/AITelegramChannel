using Telegram.Bots;
using Telegram.Bots.Requests;

namespace AiTelegramChannel.ServerHost.Telegram;

public class TelegramMessengerClient : ITelegramMessengerClient
{
    private readonly IBotClient _botClient;

    public TelegramMessengerClient(IBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMessage(long chatId, string message)
    {
        await _botClient.HandleAsync(new SendText(chatId, message));
    }
}