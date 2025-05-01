using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
	public class JwtService(IConfiguration _config)
	{
		public string GenerateToken(User user)
		{
			var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
			var issuer = _config["Jwt:Issuer"];
			var audience = _config["Jwt:Audience"];
			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Name, user.Name),
		};

			var token = new JwtSecurityToken(
					issuer: issuer,
					audience: audience,
					claims: claims,
					expires: DateTime.UtcNow.AddMinutes(15),
					signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}