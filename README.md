# AITelegramChannel
[![Telegram channel](https://icons-for-free.com/iconfiles/ico/128/messenger+social+telegram+icon-1320194696007326491.ico "Telegram channel")](https://t.me/ChatGptUAJokes "Telegram channel")

[Цікаві факти від ChatGPT](https://t.me/ChatGptUAJokes "Цікаві факти від ChatGPT")

https://t.me/ChatGptUAJokes

## Basic functionality
This is .NET 6 microservice that will call ChatGPT with configured message and after that post the response to a Telegram channel.

## Unplash integration
Based on response from ChatGPT it can ask ChatGPT again to provide a search query and after that using this search query it will call Unplash to find a related image. In the end it will make a post to the Telegram channel with an image.

## Logging
In the nlog.config it's possible to provide connection string to the MS SQL database.
Example of connection string:
`Data Source=localhost;Initial Catalog=LogDb;User ID=admin;Password=admin;TrustServerCertificate=true;`

But the table should be created before:
```sql
CREATE TABLE NLog(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    CreatedOn datetime NOT NULL,
    Level nvarchar(10),
    Message nvarchar(max),
    StackTrace nvarchar(max),
    Exception nvarchar(max),
    Logger nvarchar(255),
    Url nvarchar(255)
);
```

## Configuration
Example of configuration:
appsettings.json
```json
{
  "OpenAi": {
    "ApiKey": "sk-2dqYDm3NWP2aEDSEFe9ta5Twqdq3fg33sfdGewgJnowLyhpE"
  },
  "TelegramSettings": {
    "ChatId": -1001223417913,
    "Token": "6789012345:AAF3123yp-CYoJ47-SpTJrzaXdd3aqqYrff"
  },
  "UnsplashSettings": {
    "BaseUrl": "https://api.unsplash.com",
    "ClientId": ""
  },
  "PostsGeneratorBackgroundJobSettings": {
    "Enabled": true,
    "Message": "Tell small interesting story",
    "DelayInMinutesFrom": 20,
    "DelayInMinutesTo": 90,
    "GenerateImages": false
  }
}
```

Setting  | Definition
------------- | -------------
OpenAi:ApiKey | ApiKey that needs to be generated here: https://platform.openai.com/account/api-keys
TelegramSettings:ChatId | Can be extracted using https://t.me/JsonDumpBot
TelegramSettings:Token | Can be extracted using https://t.me/BotFather
UnsplashSettings:BaseUrl | Base url of Unsplash according to https://unsplash.com/documentation
UnsplashSettings:ClientId | Can be extracted from https://unsplash.com/oauth/applications
PostsGeneratorBackgroundJobSettings:Enabled | Can enable/disable posts generation
PostsGeneratorBackgroundJobSettings:Message | Message that will be passed to the ChatGPT
PostsGeneratorBackgroundJobSettings:DelayInMinutesFrom | Minumum delay in minutes that job will wait after posting before creating a new post
PostsGeneratorBackgroundJobSettings:DelayInMinutesTo | Maximum delay in minutes that job will wait after posting before creating a new post
PostsGeneratorBackgroundJobSettings:GenerateImages | Determines if we need just a simple message in the Telegram or it should be with image

## Running in Docker
Don't forget to update appsettings.json before building an image

Run `docker build -t ai-telegram-channel .` in the root directory where Dockerfile located

Run `docker run -d -p 8080:80 ai-telegram-channel`
