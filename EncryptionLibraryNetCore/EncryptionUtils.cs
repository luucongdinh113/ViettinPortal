using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;

namespace EncryptionLibrary
{
    public static class EncryptionUtils
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        public extern static bool GetVolumeInformation(string vol, StringBuilder name, int nameSize, out uint serialNum, out uint maxNameLen, out uint flags, StringBuilder fileSysName, int fileSysNameSize);

        //Get PC Name
        [DllImport("kernel32.dll")]
        public static extern int GetComputerName(StringBuilder lpBuffer, ref int nsize);

        private const string CryptoVIKey = "@1B2c3D4e5F6g7H8";

        private static Assembly _overrideAssembly;
        public static void SetOverrideAssembly(Assembly assembly)
        {
            _overrideAssembly = assembly;
        }

        public static string GetApplicationId()
        {
            var checkAssembly = _overrideAssembly;
            if (checkAssembly == null)
            {
                checkAssembly = Assembly.GetEntryAssembly();
            }

            string applicationId = checkAssembly
                .GetCustomAttributes(typeof(ApplicationIdAttribute), false)
                .Cast<ApplicationIdAttribute>().FirstOrDefault().ApplicationId;

            return applicationId;
        }

        public static string GetApplicationName()
        {
            var checkAssembly = _overrideAssembly;
            if (checkAssembly == null)
            {
                checkAssembly = Assembly.GetEntryAssembly();
            }

            string applicationName = checkAssembly
                .GetCustomAttributes(typeof(ApplicationNameAttribute), false)
                .Cast<ApplicationNameAttribute>().FirstOrDefault().ApplicationName;

            return applicationName;
        }

        public static List<string> GetAuthorizedProtocols()
        {
            List<string> result = new List<string>();
            // find a way to have Analyzer assembly

            var checkAssembly = _overrideAssembly;
            if (checkAssembly == null)
            {
                checkAssembly = Assembly.GetEntryAssembly();
            }

            string protocols = checkAssembly
                .GetCustomAttributes(typeof(AuthorizedAnalyzerProtocolsAttribute), false)
                .Cast<AuthorizedAnalyzerProtocolsAttribute>().FirstOrDefault().AuthorizedAnalyzerProtocols;
            result.AddRange(protocols.Split(','));
            return result;
        }


        public static string GetCustomerId()
        {
            var checkAssembly = _overrideAssembly;
            if (checkAssembly == null)
            {
                checkAssembly = Assembly.GetEntryAssembly();
            }

            var customAttrs = checkAssembly
                .GetCustomAttributes(typeof(CustomerIdAttribute), false);

            if (customAttrs != null && customAttrs.Length > 0)
            {
                string customerId = customAttrs.Cast<CustomerIdAttribute>().FirstOrDefault().CustomerId;
                return customerId;
            }

            return null;
        }

        public static bool CheckSerial(string crackPass, string serial, string applicationName, string applicationId)
        {
            if (crackPass.Equals(MD5EncryptData(serial + "-" + applicationName + "-" + applicationId)))
            {
                return true;
            }

            return false;
        }

        public static string MD5EncryptData(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            // convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] hash = md5Hash.ComputeHash(plainTextBytes);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
            }

            string temp = sb.ToString();
            StringBuilder result = new StringBuilder();
            int indexStr = 0;
            while(indexStr < temp.Length)
            {
                if (indexStr + 6 < temp.Length)
                {
                    result.Append(temp.Substring(indexStr, 6));
                }
                else
                {
                    result.Append(temp.Substring(indexStr, temp.Length - indexStr));
                }
                result.Append("-");

                indexStr += 6;
            }

            if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }

            return result.ToString();
        }

        public static string EncryptData(string plainText, string appName, string applicationId)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(appName, Encoding.ASCII.GetBytes(applicationId)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(CryptoVIKey));

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

        public static string Decrypt(string encryptedText, string appName, string applicationId)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(appName, Encoding.ASCII.GetBytes(applicationId)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(CryptoVIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
