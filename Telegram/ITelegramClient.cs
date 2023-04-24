namespace AiTelegramChannel.ServerHost.Telegram;

public interface ITelegramClient
{
    Task PostMessageWithImage(string text, Uri imageUrl);

    Task PostSimpleMessage(string text);
}