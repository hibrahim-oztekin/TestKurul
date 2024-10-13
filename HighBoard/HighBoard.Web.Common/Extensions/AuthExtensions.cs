
using HighBoard.Core.Enums;
using HighBoard.Core.Helpers;
using HighBoard.Domain.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HighBoard.Web.Common.Extensions;

public class AuthExtensions : IAuthExtensions
{
    private readonly IJwtSettings _jwtSettings;

    public AuthExtensions(IJwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new("UserName", user.UserName),
        };

        var userRole = new IdCode(RoleOption.Developer.GetEnumDescriptionAsGuid(), RoleOption.Developer.ToString());
        claims.Add(new Claim("CurrentRole", userRole.Serialize()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var token = new JwtSecurityToken(
            _jwtSettings.ValidIssuer,
            _jwtSettings.ValidAudience,
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpirationInMinutes)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
