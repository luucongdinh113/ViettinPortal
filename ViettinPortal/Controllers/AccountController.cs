using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mita.Business.BusinessEnum;
using Mita.Business.BusinessObjects;
using Mita.Business.BusinessServices;
using Mita.Business.Helpers;
using MitaInternal.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
//using SpecLab.Business;

namespace ViettinPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public class LoginResponse
        {
            public string Email { get; set; }
            public string FullName { get; set; }
            public List<UserRightCode> Roles { get; set; }
            public string Description { get; set; }
        }

        public class ChangePassRequest
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult Authenticate([FromBody] LoginViewModel userDto)
        {
            try
            {
                var user = UserService.GetInstance().Login(userDto.Email, userDto.Password);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(CommonConstant.ApplicationId);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info (without password) and token to store client side
                return Ok(new
                {
                    result = new LoginResponse()
                    {
                        Email = user.UserId,
                        FullName = user.FullName,
                        Description = "",
                        Roles = user.Rights
                    },
                    Token = tokenString
                });
            }
            catch (BusinessException be)
            {
                return BadRequest(new { message = be.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost()]
        public IActionResult ChangePass([FromBody] ChangePassRequest request)
        {
            try
            {
                if (!UserService.GetInstance().ChangePassword(
                    new UserService.ChangePassParams()
                    {
                        UserId = User.Identity.Name,
                        NewPass = request.NewPassword,
                        OldPass = request.OldPassword
                    }))
                {
                    return BadRequest(new { message = "Đổi mật khẩu thất bại" });
                }

                return Ok(new
                {
                    result = "Đổi pass thành công",
                });
            }
            catch (BusinessException be)
            {
                return BadRequest(new { message = be.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
