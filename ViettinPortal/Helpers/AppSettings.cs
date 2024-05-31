using System;
using System.Globalization;

namespace WebApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string HostAddress { get; set; }
        public string DatabaseName { get; set; }
        public string EncryptUser { get; set; }
        public string EncryptPass { get; set; }
    }
}