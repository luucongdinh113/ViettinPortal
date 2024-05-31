using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mita.Business.BusinessEnum;
using Mita.Business.BusinessObjects;
using Mita.Business.BusinessServices;
using Mita.Business.BusinessServices.Config;
using Mita.Business.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ViettinPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GenKeyController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string DMSGenerateKeyId = "DMS client";
        const string BloodBankGenerateKeyId = "Blood Bank client";
        const string QCSGenerateKeyId = "QC client";

        public class GetApplicationListRequest
        {
            public string ApplicationFilter { get; set; }
        }

        public class GenerateKeyRequest
        {
            [Required]
            public string GenerateKeyId { get; set; }

            [Required]
            public string Serial { get; set; }
        }

        public class GenerateKeyInstrumentRequest
        {
            [Required]
            public string GenerateKeyId { get; set; }

            [Required]
            public string InstrumentId { get; set; }

            [Required]
            public string InstrumentExpire { get; set; }
        }

        public class ApplicationInfo
        {
            public string DisplayText { get; set; }
            public string GenerateKeyId { get; set; }
        }

        private class ApplicationInfoSort : IComparer<ApplicationInfo>
        {
            public int Compare(ApplicationInfo x, ApplicationInfo y)
            {
                return x.DisplayText.CompareTo(y.DisplayText);
            }
        }

        [HttpPost()]
        public IActionResult GetApplicationList([FromBody] GetApplicationListRequest request)
        {
            try
            {
                var listApplication = ApplicationKeyService.GetInstance().GetListApplicationKey(
                    new ApplicationKeyService.SearchCriteria
                    {
                        ApplicationFilter = request.ApplicationFilter
                    })
                    .Select(x => new ApplicationInfo()
                    {
                        GenerateKeyId = CommonUtils.EncryptData($"{x.ApplicationId}|{x.ApplicationName}|{DateTime.Today.Day}"),
                        DisplayText = x.DisplayText
                    })
                    .ToList();

                listApplication.Sort(new ApplicationInfoSort());

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

        [HttpPost()]
        public IActionResult GetApplicationClientList([FromBody] GetApplicationListRequest request)
        {
            try
            {
                var listApplication = new List<ApplicationInfo>()
                {
                    new ApplicationInfo()
                    {
                        GenerateKeyId = DMSGenerateKeyId,
                        DisplayText = DMSGenerateKeyId
                    },

                    new ApplicationInfo()
                    {
                        GenerateKeyId = BloodBankGenerateKeyId,
                        DisplayText = BloodBankGenerateKeyId
                    },

                    new ApplicationInfo()
                    {
                        GenerateKeyId = QCSGenerateKeyId,
                        DisplayText = QCSGenerateKeyId
                    },
                };

                listApplication = listApplication
                    .Where(x => x.DisplayText.Contains(request.ApplicationFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                listApplication.Sort(new ApplicationInfoSort());

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

        [HttpPost()]
        public IActionResult Generate([FromBody] GenerateKeyRequest generateKeyRequest)
        {
            try
            {
                var xxx = CommonUtils.Decrypt(generateKeyRequest.GenerateKeyId);
                var appId = xxx.Split('|')[0];

                var selectedApp = ApplicationKeyService.GetInstance().GetApplicationKey(appId);
                if (selectedApp == null)
                {
                    throw new BusinessException(AppErrorCode.ApplicationNotExists);
                }

                var generateKeyId = generateKeyRequest.GenerateKeyId.Trim();
                var serial = generateKeyRequest.Serial.Trim();

                var serialUnlockCode = ApplicationKeyService.GetInstance().GetSerialUnlock(appId, serial);

                string titleWarning = $"[UnlockCode] Serial unlock code - {selectedApp.DisplayText}";

                string body = $@"Serial: [{generateKeyRequest.Serial}]
Unlock code: [{serialUnlockCode}]";

                List<string> toAddress = new List<string> { this.HttpContext.User.Identity.Name };

                EmailSendService.GetInstance().SendEmail(toAddress, titleWarning, body,
                    EmailSendingConfig.GetSystemConfig().EmailUsername,
                    EmailSendingConfig.GetSystemConfig().EmailPassword,
                    new List<EmailSendService.Attachment>(), false);

                return Ok(new
                {
                    result = "Vui lòng kiểm tra mail",
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
        public IActionResult GenKeyInstrument([FromBody] GenerateKeyInstrumentRequest generateKeyRequest)
        {
            try
            {
                var xxx = CommonUtils.Decrypt(generateKeyRequest.GenerateKeyId);
                var appId = xxx.Split('|')[0];

                var selectedApp = ApplicationKeyService.GetInstance().GetApplicationKey(appId);
                if (selectedApp == null)
                {
                    throw new BusinessException(AppErrorCode.ApplicationNotExists);
                }

                var generateKeyId = generateKeyRequest.GenerateKeyId.Trim();
                var instrumentId = generateKeyRequest.InstrumentId.Trim();
                var expireDate = generateKeyRequest.InstrumentExpire.Trim();

                if (!DateTime.TryParseExact(expireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime checkExpire))
                {
                    throw new ArgumentException("InstrumentExpire is not datetime format !");
                }

                var serialUnlockCode = ApplicationKeyService.GetInstance().GetInstrumentExpireKey(appId, instrumentId, checkExpire);

                string titleWarning = $"[UnlockCode] Instrument unlock code - {selectedApp.DisplayText}";
                string body = $@"InstrumentId: [{generateKeyRequest.InstrumentId}]
Unlock code: [{serialUnlockCode}]";

                List<string> toAddress = new List<string> { this.HttpContext.User.Identity.Name };

                EmailSendService.GetInstance().SendEmail(toAddress, titleWarning, body,
                    EmailSendingConfig.GetSystemConfig().EmailUsername,
                    EmailSendingConfig.GetSystemConfig().EmailPassword,
                    new List<EmailSendService.Attachment>(), false);

                return Ok(new
                {
                    result = "Vui lòng kiểm tra mail",
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
        public IActionResult GenerateClient([FromBody] GenerateKeyRequest generateKeyRequest)
        {
            try
            {
                string serialUnlockCode = string.Empty;
                var generateKeyId = generateKeyRequest.GenerateKeyId.Trim();
                var serial = generateKeyRequest.Serial.Trim();
                switch (generateKeyId)
                {
                    case DMSGenerateKeyId:
                        serialUnlockCode = ApplicationKeyService.GetInstance().GetClientSerialUnlock(serial, 8);
                        break;
                    case BloodBankGenerateKeyId:
                        serialUnlockCode = ApplicationKeyService.GetInstance().GetClientSerialUnlock(serial, 8);
                        break;
                    case QCSGenerateKeyId:
                        serialUnlockCode = ApplicationKeyService.GetInstance().GetClientSerialUnlock(serial, 9);
                        break;
                }

                string titleWarning = $"[UnlockCode] Serial unlock code - {generateKeyRequest.GenerateKeyId}";

                string body = $@"Serial: [{generateKeyRequest.Serial}]
Unlock code: [{serialUnlockCode}]";

                List<string> toAddress = new List<string> { this.HttpContext.User.Identity.Name };

                EmailSendService.GetInstance().SendEmail(toAddress, titleWarning, body,
                    EmailSendingConfig.GetSystemConfig().EmailUsername,
                    EmailSendingConfig.GetSystemConfig().EmailPassword,
                    new List<EmailSendService.Attachment>(), false);

                return Ok(new
                {
                    result = "Vui lòng kiểm tra mail",
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
