using Microsoft.AspNetCore.Http;
using Mita.Business.BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Mita.Business.Helpers
{
    public static class SessionUtils
    {
        public const string SessionKeyUserId = "LoginUserInfo";

        public static bool Exist(HttpContext httpContext, string key)
        {
            if (httpContext != null)
            {
                byte[] temp = null;
                return httpContext.Session.TryGetValue(key, out temp);
            }
            return false;
        }

        public static T GetSession<T>(HttpContext httpContext, string key)
        {
            if (Exist(httpContext, key))
            {
                var serializeContent = httpContext.Session.GetString(key);
                return CommonUtils.Deserialize<T>(serializeContent);
            }
            return default(T);
        }

        public static void SetSession<T>(HttpContext httpContext, string key, T val)
        {
            string serializeObject = null;

            var serializer = new XmlSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                var settings = new XmlWriterSettings();
                settings.Encoding = UTF8Encoding.UTF8;
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (var xmlTextWriter = XmlWriter.Create(memoryStream, settings))
                {
                    serializer.Serialize(xmlTextWriter, val);
                }
                serializeObject = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            httpContext.Session.SetString(key, CommonUtils.Serialize(val));

        }
    }
}