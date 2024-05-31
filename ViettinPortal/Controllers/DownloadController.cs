using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ViettinPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class DownloadController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AutoMaintenance()
        {
            //byte[] result = null;
            //using (var stream = new FileStream(@"D:\Test\AutoMaintenanceService.zip", 
            //    FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    return File(stream, "application /octet-stream", "AutoMaintenanceService.zip"); // returns a FileStreamResult
            //}

            byte[] result = await ReadDataAsync(@"D:\Test\AutoMaintenanceService.zip");
            return File(result, "application /octet-stream", "AutoMaintenanceService.zip"); // returns a FileStreamResult
        }

        private async Task<byte[]> ReadDataAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                byte[] _processingReceived = new byte[0];
                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    byte[] tempBytes = new byte[_processingReceived.Length + numRead];
                    Buffer.BlockCopy(_processingReceived, 0, tempBytes, 0, _processingReceived.Length);
                    Buffer.BlockCopy(buffer, 0, tempBytes, _processingReceived.Length, numRead);
                    _processingReceived = tempBytes;
                }

                return _processingReceived;
            }
        }
    }
}
