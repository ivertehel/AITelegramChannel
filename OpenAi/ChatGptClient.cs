using AiTelegramChannel.ServerHost.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace AiTelegramChannel.ServerHost.OpenAi;

public class ChatGptClient : IChatGptClient
{
    private readonly IOpenAIService _openAIService;
    private readonly ILogger<ChatGptClient> _logger;

    public ChatGptClient(IOpenAIService openAIService, ILogger<ChatGptClient> logger)
    {
        _openAIService = openAIService;
        _logger = logger;
    }

    public async Task<string> SendMessage(string message)
    {
        _logger.TraceEnter(argument: message);
        var completionResult = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(message),
            },
            Model = Models.ChatGpt3_5Turbo
        });

        if (!completionResult.Successful)
        {
            var ex = new Exception(completionResult?.Error?.Message);
            _logger.TraceError(ex);
            throw ex;
        }

        return _logger.LogExit(result: string.Join(" ", completionResult.Choices.Select(s => s.Message.Content)));
    }
}