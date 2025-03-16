using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.JwtProvider;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options; 
    
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    
    public string CreateToken(int userId)
    {
        Claim[] claims = new Claim[]{
            new Claim("UserId",userId.ToString())
        };

        string? tokenKeyString = _options.TokenKey;

        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                tokenKeyString != null ? tokenKeyString : ""
            )
        );

        SigningCredentials creadentials = new SigningCredentials(
            tokenKey,
            SecurityAlgorithms.HmacSha512Signature
        );

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = creadentials,
            Expires = DateTime.Now.AddHours(_options.ExpitesHours)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }
    
}