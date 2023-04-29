using AiTelegramChannel.ServerHost.Options;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AiTelegramChannel.ServerHost.Components.GetToken;

public class GetTokenRequestHandler : IRequestHandler<GetTokenRequest, Result<string>>
{
    private readonly ApiSettings _apiSettings;

    public GetTokenRequestHandler(IOptions<ApiSettings> apiSettings)
    {
        _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));
    }

    public async Task<Result<string>> Handle(GetTokenRequest request, CancellationToken cancellationToken)
    {
        if (_apiSettings.ApiKey == request.ApiKey)
        {
            return await Task.FromResult(GenerateJwtToken(_apiSettings.ApiKey));
        }

        return Result.Fail("User can't be authenticated. Api key is invalid");
    }

    private string GenerateJwtToken(string apiKey)
    {
        var now = DateTime.UtcNow;

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: "AiTelegramChannel",
            audience: "AiTelegramChannel",
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey)), SecurityAlgorithms.HmacSha256),
            notBefore: DateTime.UtcNow,
            expires: now.AddDays(1)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}