using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ViettinPortal.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestAuthorizeController : ControllerBase
    {
        public class LoginViewModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }

        [HttpPost()]
        [Authorize]
        public IActionResult Test([FromBody]LoginViewModel model)
        {
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}