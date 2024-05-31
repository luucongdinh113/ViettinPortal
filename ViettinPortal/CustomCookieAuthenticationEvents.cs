using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MitaInternal
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task SignedIn(CookieSignedInContext context)
        {
            return base.SignedIn(context);
        }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            return base.ValidatePrincipal(context);
        }

        public override Task SigningOut(CookieSigningOutContext context)
        {
            return base.SigningOut(context);
        }

        //public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        //{
        //var userPrincipal = context.Principal;

        //await context.HttpContext.SignOutAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme);

        //// Look for the LastChanged claim.
        //var lastChanged = (from c in userPrincipal.Claims
        //                   where c.Type == "LastChanged"
        //                   select c.Value).FirstOrDefault();

        //if (string.IsNullOrEmpty(lastChanged) ||
        //    !_userRepository.ValidateLastChanged(lastChanged))
        //{
        //    context.RejectPrincipal();

        //    await context.HttpContext.SignOutAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme);
        //}
        //}
    }
}
