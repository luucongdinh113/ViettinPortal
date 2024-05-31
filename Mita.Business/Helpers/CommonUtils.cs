using System.IO;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Mita.Business.BusinessEnum;
using System.Xml.Serialization;
using System.Xml;
using Mita.Business.BusinessObjects;
//using Spring.Context.Support;

namespace Mita.Business.Helpers
{
    public static class CommonUtils
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            
            return value.ToString();
        }

        public static T GetEnumFromValue<T>(string value)
        {
            return (T)Enum.Parse(typeof (T), value);
        }

        public static List<UserRightCode> GetListDefaultUserRightCode()
        {
            List<UserRightCode> listDefault = new List<UserRightCode>(new UserRightCode[]
            {
                UserRightCode.R00100,
                UserRightCode.R00101
            });

            return listDefault;
        }

        public static string EncryptData(string plainText)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                byte[] keyBytes = new Rfc2898DeriveBytes(CommonConstant.ApplicationName, Encoding.ASCII.GetBytes(CommonConstant.ApplicationId)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(CommonConstant.CryptoVIKey));

                byte[] cipherTextBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
                return Convert.ToBase64String(cipherTextBytes);
            }
            return string.Empty;
        }

        public static string Decrypt(string encryptedText)
        {
            if (!string.IsNullOrEmpty(encryptedText))
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(CommonConstant.ApplicationName, Encoding.ASCII.GetBytes(CommonConstant.ApplicationId)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(CommonConstant.CryptoVIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            return string.Empty;
        }

        public static string GetErrorMessage(AppErrorCode code, params object[] inputParams)
        {
            string errorMessage = String.Format(GetEnumDescription(code), inputParams);
            return errorMessage;
        }

        public static string GetErrorMessage(BusinessException businessException)
        {
            string errorMessage = string.Empty;
            if(businessException.ErrorParams != null)
            {
                errorMessage = String.Format(GetEnumDescription(businessException.ErrorCode), businessException.ErrorParams);
            }
            else
            {
                errorMessage = String.Format(GetEnumDescription(businessException.ErrorCode));
            }
            
            return errorMessage;
        }

        public static string Serialize<T>(T val)
        {
            string serializeObject = null;
            var type = typeof(T);
            if (type.IsSerializable)
            {
                var serializer = new XmlSerializer(type);
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
            }
            else
            {
                throw new ArgumentException("Parameter val is not IsSerializable !");
            }
            return serializeObject;
        }

        public static T Deserialize<T>(string serializeContent)
        {
            var type = typeof(T);
            if (type.IsSerializable)
            {
                var serializer = new XmlSerializer(type);
                using (var readStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(serializeContent)))
                {
                    var reader = new XmlTextReader(readStream);
                    return (T)serializer.Deserialize(reader);
                }
            }
            else
            {
                throw new ArgumentException("Type is not IsSerializable !");
            }
        }
    }
}
