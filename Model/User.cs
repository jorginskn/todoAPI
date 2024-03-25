using System.Security.Claims;

namespace todoAPI.Model
{
    public class User
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }


        public static User FromClaimsPrincipal(ClaimsPrincipal userPrincipal)
        {
            var usuario = new User();

            var userNameClaim = userPrincipal.Identities.FirstOrDefault().Claims.FirstOrDefault();

             if (userNameClaim != null)
           {
                usuario.Name = userNameClaim.Type;
                usuario.Id = userNameClaim.Value;
            }

            var userEmailClaim = userPrincipal.FindFirst(ClaimTypes.Email);
            if (userEmailClaim != null)
            {
                usuario.Email = userEmailClaim.Value;
            }

            return usuario;
        }
    }
}
