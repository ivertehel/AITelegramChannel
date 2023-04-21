# AITelegramChannel
This is .NET 6 microservice that will call ChatGPT with configured message and after that post the response to a telegram channel.

## Configuration
Configure the appsettings.json
```json
{
  "OpenAi": {
    "ApiKey": "sk-2dqYDm3NWP2aEDSEFe9ta5Twqdq3fg33sfdGewgJnowLyhpE"
  },
  "TelegramSettings": {
    "ChatId": -1001223417913,
    "Token": "6789012345:AAF3123yp-CYoJ47-SpTJrzaXdd3aqqYrff"
  },
  "PostsGeneratorBackgroundJobSettings": {
    "Enabled": true,
    "Message": "Tell small interesting story",
    "DelayInMinutesFrom": 20,
    "DelayInMinutesTo": 90
  }
}
```

Setting  | Definition
------------- | -------------
OpenAi:ApiKey | ApiKey that needs to be generated here: https://platform.openai.com/account/api-keys
TelegramSettings:ChatId | Can be extracted using https://t.me/JsonDumpBot
TelegramSettings:Token | Can be extracted using https://t.me/BotFather
PostsGeneratorBackgroundJobSettings:Enabled | Can enable/disable posts generation
PostsGeneratorBackgroundJobSettings:Message | Message that will be passed to the ChatGPT
PostsGeneratorBackgroundJobSettings:DelayInMinutesFrom | Minumum delay in minutes that job will wait after posting before creating a new post
PostsGeneratorBackgroundJobSettings:DelayInMinutesTo | Maximum delay in minutes that job will wait after posting before creating a new post

## Running in Docker
Don't forget to update appsettings.json before building an image

Run `docker build -t ai-telegram-channel .` in the root directory where Dockerfile located

Run `docker run -d -p 8080:80 ai-telegram-channel`
