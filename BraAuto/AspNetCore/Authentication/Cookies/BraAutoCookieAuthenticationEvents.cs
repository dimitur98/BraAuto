using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authentication.Cookies;
using BraAuto.Helpers.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using CloudinaryDotNet.Actions;

namespace BraAuto.AspNetCore.Authentication.Cookies
{
    public class BraAutoCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var authType = context.Principal.Identity.AuthenticationType;
            var email = context.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var claims = context.Principal.Claims.ToList();
            var username = context.Principal.Identity.Name;

            if ((authType.ToLower() == "google" || authType.ToLower() == "facebook") && !string.IsNullOrEmpty(email))
            {
                var user = Db.Users.GetByEmail(email, isPasswordRequired: false);

                if (user == null)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(user.Username)) 
                { 
                    username = user.Username;
                    claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, username)
                    };
                }
            }


            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = Db.Users.GetByUsername(username);

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
                        Db.UserInRoles.LoadUserRoles(user.Roles);

                        var roles = user.Roles.Select(uir => uir.UserRole.Name.Replace(" ", "-").ToLower());

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
