using System;
using System.Reflection;
using System.Runtime.Serialization;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mita.Business.BusinessObjects;
using Mita.Business.BusinessServices;
using Mita.Business.Model;

namespace ViettinPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class MaintenanceController : ControllerBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [DataContract]
        public class TestConnectionResponse
        {
            [DataMember]
            public string AckTime { get; set; }
        }

        [DataContract]
        public class ReportErrorResponse
        {
            [DataMember]
            public string AckTime { get; set; }
        }

        [DataContract]
        public class TestConnectionRequest
        {
            [DataMember]
            public string SiteName { get; set; }
            [DataMember]
            public string HostName { get; set; }
        }

        [DataContract]
        public class ReportErrorRequest
        {
            [DataMember]
            public string SiteName { get; set; }
            [DataMember]
            public string HostName { get; set; }
            [DataMember]
            public string ErrorMessage { get; set; }
        }

        public class MaintenanceBasicRequest
        {
            public string SiteCode { get; set; }
            public string SiteSendInfo { get; set; }
            public int CountPatient { get; set; }
            public int CountResult { get; set; }
            public DateTime MinDateIN { get; set; }
            public DateTime MaxDateIN { get; set; }
        }

        public class PatientMonthStatRequest
        {
            public string SiteCode { get; set; }
            public string SiteSendInfo { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int CountPatient { get; set; }
        }

        public class ResultMonthStatRequest
        {
            public string SiteCode { get; set; }
            public string SiteSendInfo { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public string TestCode { get; set; }
            public string TestName { get; set; }
            public int CountTest { get; set; }
        }

        public class ResultInstrumentMonthStatRequest
        {
            public string SiteCode { get; set; }
            public string SiteSendInfo { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int InstrumentID { get; set; }
            public string InstrumentName { get; set; }
            public string TestCode { get; set; }
            public string TestName { get; set; }
            public int CountTest { get; set; }
        }

        public class ResultDepartmentMonthStatRequest
        {
            public string SiteCode { get; set; }
            public string SiteSendInfo { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public string LocationID { get; set; }
            public string LocationName { get; set; }
            public string TestCode { get; set; }
            public string TestName { get; set; }
            public int CountTest { get; set; }
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult UpdateResultDepartmentMonthStat([FromBody]ResultDepartmentMonthStatRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    StatMonthlyResultDepartmentService.GetInstance().SaveStatMonthlyResultDepartment(context,
                        new StatMonthlyResultDepartment()
                        {
                            SiteCode = request.SiteCode,
                            CountTest = request.CountTest,
                            StatMonth = new DateTime(request.Year, request.Month, 1),
                            SiteSendInfo = request.SiteSendInfo,
                            LocationID = request.LocationID,
                            LocationName = request.LocationName,
                            TestCode = request.TestCode,
                            TestName = request.TestName,
                            SysId = Guid.NewGuid(),
                            InTime = DateTime.Now,
                            UpdateTo = DateTime.Now,
                            UserI = "system",
                            UserU = "system",
                        });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult UpdateResultInstrumentMonthStat([FromBody]ResultInstrumentMonthStatRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    StatMonthlyResultInstrumentService.GetInstance().SaveStatMonthlyResultInstrument(context,
                        new StatMonthlyResultInstrument()
                        {
                            SiteCode = request.SiteCode,
                            CountTest = request.CountTest,
                            StatMonth = new DateTime(request.Year, request.Month, 1),
                            SiteSendInfo = request.SiteSendInfo,
                            InstrumentID = request.InstrumentID,
                            InstrumentName = request.InstrumentName,
                            TestCode = request.TestCode,
                            TestName = request.TestName,
                            SysId = Guid.NewGuid(),
                            InTime = DateTime.Now,
                            UpdateTo = DateTime.Now,
                            UserI = "system",
                            UserU = "system",
                        });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult UpdateResultInstrumentMonthCombineStat([FromBody]ResultInstrumentMonthStatRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    StatMonthlyResultInstrumentCombineService.GetInstance().SaveStatMonthlyResultInstrumentCombine(context,
                        new StatMonthlyResultInstrumentCombine()
                        {
                            SiteCode = request.SiteCode,
                            CountTest = request.CountTest,
                            StatMonth = new DateTime(request.Year, request.Month, 1),
                            SiteSendInfo = request.SiteSendInfo,
                            InstrumentID = request.InstrumentID,
                            InstrumentName = request.InstrumentName,
                            TestCode = request.TestCode,
                            TestName = request.TestName,
                            SysId = Guid.NewGuid(),
                            InTime = DateTime.Now,
                            UpdateTo = DateTime.Now,
                            UserI = "system",
                            UserU = "system",
                        });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult UpdateResultMonthStat([FromBody]ResultMonthStatRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    StatMonthlyResultService.GetInstance().SaveStatMonthlyResult(context,
                        new StatMonthlyResult()
                        {
                            SiteCode = request.SiteCode,
                            CountTest = request.CountTest,
                            StatMonth = new DateTime(request.Year, request.Month, 1),
                            SiteSendInfo = request.SiteSendInfo,
                            TestCode = request.TestCode,
                            TestName = request.TestName,
                            SysId = Guid.NewGuid(),
                            InTime = DateTime.Now,
                            UpdateTo = DateTime.Now,
                            UserI = "system",
                            UserU = "system",
                        });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult UpdatePatientMonthStat([FromBody]PatientMonthStatRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    StatMonthlyPatientService.GetInstance().SaveStatMonthlyPatient(context,
                        new StatMonthlyPatient()
                        {
                            SiteCode = request.SiteCode,
                            CountPatient = request.CountPatient,
                            StatMonth = new DateTime(request.Year, request.Month, 1),
                            SiteSendInfo = request.SiteSendInfo,
                            SysId = Guid.NewGuid(),
                            InTime = DateTime.Now,
                            UpdateTo = DateTime.Now,
                            UserI = "system",
                            UserU = "system"
                        });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult UpdateMaintenanceBasic([FromBody]MaintenanceBasicRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    MaintenanceBasicService.GetInstance().SaveDailyMaintenanceBasic(context,
                        new DailyMaintenanceBasic()
                        {
                            SiteCode = request.SiteCode,
                            CountPatient = request.CountPatient,
                            CountResult = request.CountResult,
                            CheckDate = DateTime.Now,
                            MaxDateIN = request.MaxDateIN,
                            MinDateIN = request.MinDateIN,
                            SiteSendInfo = request.SiteSendInfo,
                            SysId = Guid.NewGuid()
                        });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult TestConnection([FromBody]TestConnectionRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    NodeMonitorService.GetInstance().SyncNodeMonitor(context, new NodeMonitor()
                    {
                        SysId = Guid.NewGuid(),
                        HostName = request.HostName,
                        SiteName = request.SiteName,
                        CheckTime = DateTime.Now
                    });

                    return Ok(new
                    {
                        result = new TestConnectionResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult ReportError([FromBody]ReportErrorRequest request)
        {
            try
            {
                using (var context = MitaContext.GetContextInstance())
                {
                    NodeErrorService.GetInstance().SaveReportError(context, new NodeError()
                    {
                        SysId = Guid.NewGuid(),
                        HostName = request.HostName,
                        SiteName = request.SiteName,
                        ErrorTime = DateTime.Now,
                        ErrorMessage = request.ErrorMessage
                    });

                    return Ok(new
                    {
                        result = new ReportErrorResponse()
                        {
                            AckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        },
                    });
                }
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
