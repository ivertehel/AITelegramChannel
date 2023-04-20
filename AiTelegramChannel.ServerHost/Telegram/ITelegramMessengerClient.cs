namespace AiTelegramChannel.ServerHost.Telegram;

public interface ITelegramMessengerClient
{
    Task SendMessage(long chatId, string message);
}