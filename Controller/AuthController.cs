using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        // Verifica se as credenciais são válidas
        if (model.Username != "usuario" || model.Password != "senha")
        {
            // Se as credenciais não são válidas, retorna um erro de não autorizado
            return Unauthorized();
        }

        // Gera o token JWT
        var token = GenerateJwtToken();

        // Retorna o token JWT
        return Ok(new { token });
    }

    private string GenerateJwtToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "usuario"),
                new Claim(ClaimTypes.Role, "admin") // Você pode adicionar mais roles se necessário
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Tempo de expiração do token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
