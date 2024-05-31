using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using EncryptionLibrary;
using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mita.Business.BusinessEnum;
using Mita.Business.BusinessObjects;
using Mita.Business.BusinessServices;
using Mita.Business.BusinessServices.Config;
using Mita.Business.Helpers;
using MitaInternal;
using MitaInternal.Models.AccountViewModels;

namespace ViettinPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public class GetCustomerListRequest
        {
            public string CustomerIdFilter { get; set; }

            public string CustomerNameFilter { get; set; }
        }

        public class AddCustomerRequest
        {
            public string CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string Description { get; set; }
        }

        public class DeleteCustomerRequest
        {
            public Guid SysId { get; set; }
        }

        public IActionResult AddCustomer([FromBody]AddCustomerRequest request)
        {
            try
            {
                var customerInfo = CustomerService.GetInstance().CreateNewCustomer(
                    new CustomerService.CustomerInfo()
                    {
                        SysId = Guid.NewGuid(),
                        CustomerId = request.CustomerId,
                        CustomerName = request.CustomerName,
                        Description = request.Description
                    }, this.HttpContext.User.Identity.Name);

                return Ok(new
                {
                    result = customerInfo,
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

        public IActionResult DeleteCustomer([FromBody]DeleteCustomerRequest request)
        {
            try
            {
                CustomerService.GetInstance().DeleteCustomer(
                    request.SysId, this.HttpContext.User.Identity.Name);

                return Ok(new
                {
                    result = "",
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
        public IActionResult GetCustomerList([FromBody]GetCustomerListRequest request)
        {
            try
            {
                var listApplication = CustomerService.GetInstance().GetListCustomer(new CustomerService.SearchCriteria()
                {
                    CustomerIdFilter = request.CustomerIdFilter,
                    CustomerNameFilter = request.CustomerNameFilter
                });

                return Ok(new
                {
                    result = listApplication,
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
