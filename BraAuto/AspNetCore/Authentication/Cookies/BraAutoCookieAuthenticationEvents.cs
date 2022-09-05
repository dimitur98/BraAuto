using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authentication.Cookies;
using BraAuto.Helpers.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace BraAuto.AspNetCore.Authentication.Cookies
{
    public class BraAutoCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var username = context.Principal.Identity.Name;

            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = Users.GetByUsername(username);

                if (user != null && user.IsActive == false)
                {
                    user = null;
                }

                if (user != null)
                {
                    // set roles    
                    user.LoadUserInRoles();

                    if (!user.Roles.IsNullOrEmpty())
                    {
                        UserInRoles.LoadUserRoles(user.Roles);

                        var roles = user.Roles.Select(uir => uir.UserRole.Name.Replace(" ", "-").ToLower());

                        var claims = context.Principal.Claims.ToList();

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var identity = new ClaimsIdentity(claims, context.Principal.Identity.AuthenticationType);
                        var principal = new ClaimsPrincipal(identity);

                        context.ReplacePrincipal(principal);

                        return;
                    }
                }
            }

            context.RejectPrincipal();

            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
