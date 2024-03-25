using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using todoAPI.Model;

namespace todoAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        private readonly List<string> _excludeRoutes = new List<string>
{
    "/Auth/login"
};

        public JwtMiddleware(RequestDelegate next, string secretKey)
        {
            _next = next;
            _secretKey = secretKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;
            if (_excludeRoutes != null && _excludeRoutes.Contains(path))
            {
                await _next(context);
                return;
            }
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContext(context, token);
            }
            else
            {
                context.Response.StatusCode = 403;
                return;
            }
            await _next(context);

        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims;

                // Adiciona as claims do token ao contexto HTTP
                context.Items["User"] =  User.FromClaimsPrincipal((new ClaimsPrincipal(new ClaimsIdentity(claims)))) ;

            }
            catch
            {
                context.Response.StatusCode = 403;
                return;
            }
        }
    }
}
