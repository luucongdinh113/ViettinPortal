using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MitaInternal.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ViettinPortal.Models.DataModel;

namespace MitaInternal
{
    public interface ITestUserManager
    {

    }

    public class TestUserManager : ITestUserManager
    {
        string _connectionString;

        public TestUserManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserModel> SignIn(HttpContext httpContext, LoginViewModel user, bool isPersistent = false)
        {
            var dbUserData = new UserModel()
            {
                FullName = "Administrator",
                UserId = user.Email
            };

            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(dbUserData), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
            });

            return dbUserData;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(UserModel user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName));
            claims.Add(new Claim(ClaimTypes.Email, user.UserId + "@mitalabvn.com"));
            claims.AddRange(this.GetUserRoleClaims(user));
            return claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(UserModel user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId));
            claims.Add(new Claim(ClaimTypes.Role, "R00101"));
            return claims;
        }
    }
}
