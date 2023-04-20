using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace AiTelegramChannel.ServerHost.OpenAi;

public class ChatGptClient : IChatGptClient
{
    private readonly IOpenAIService _openAIService;

    public ChatGptClient(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<string> SendMessage(string message)
    {
        var completionResult = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(message),
            },
            Model = Models.ChatGpt3_5Turbo
        });

        if (completionResult.Successful)
        {
            return string.Join(" ", completionResult.Choices.Select(s => s.Message.Content));
        }

        return "";
    }
}