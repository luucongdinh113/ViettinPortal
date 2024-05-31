using Mita.Business.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Mita.Business.BusinessServices.Config
{
    public class EmailSendingConfig : BaseSystemConfig<EmailSendingConfig>
    {
        private const string GeneralConfigAppId = "Mita.Business.1.0";
        private const string GeneralConfigAppName = "Mita.Business";

        private const string KeyEmailPort = "EmailSendingConfig_EmailPort";
        private const string KeyEmailHost = "EmailSendingConfig_EmailHost";
        private const string KeyEmailFromAddress = "EmailSendingConfig_EmailFromAddress";
        private const string KeyEmailUsername = "EmailSendingConfig_EmailUsername";
        private const string KeyEmailPassword = "EmailSendingConfig_EmailPassword";
        private const string KeyUseDefaultCredentials = "EmailSendingConfig_UseDefaultCredentials";

        public bool UseDefaultCredentials
        {
            get
            {
                bool result = GetConfigValue(KeyUseDefaultCredentials, false);
                return result;
            }
            set
            {
                SaveConfigValue(KeyUseDefaultCredentials, value);
            }
        }

        public int EmailPort
        {
            get
            {
                int result = GetConfigValue(KeyEmailPort, 587); //25
                return result;
            }
            set
            {
                SaveConfigValue(KeyEmailPort, value);
            }
        }

        public string EmailHost
        {
            get
            {
                string result = GetConfigValue(KeyEmailHost, "mail.mitalabvn.com");
                return result;
            }
            set
            {
                SaveConfigValue(KeyEmailHost, value);
            }
        }

        public string EmailFromAddress
        {
            get
            {
                string result = GetConfigValue(KeyEmailFromAddress, "auto.noreply@mitalabvn.com");
                return result;
            }
            set
            {
                SaveConfigValue(KeyEmailFromAddress, value);
            }
        }

        public string EmailUsername
        {
            get
            {
                string defaultValue = "auto.noreply@mitalabvn.com";
                string encryptedDefaultValue = EncryptionLibrary.EncryptionUtils.EncryptData(defaultValue, GeneralConfigAppName, GeneralConfigAppId);

                string resultEncrypt = GetConfigValue(KeyEmailUsername, encryptedDefaultValue);
                string result = EncryptionLibrary.EncryptionUtils.Decrypt(resultEncrypt, GeneralConfigAppName, GeneralConfigAppId);

                return result; ;
            }
            set
            {
                string encryptedValue = EncryptionLibrary.EncryptionUtils.EncryptData(value, GeneralConfigAppName, GeneralConfigAppId);
                SaveConfigValue(KeyEmailUsername, encryptedValue);
            }
        }

        public string EmailPassword
        {
            get
            {
                string defaultValue = "Mitalab";
                string encryptedDefaultValue = EncryptionLibrary.EncryptionUtils.EncryptData(defaultValue, GeneralConfigAppName, GeneralConfigAppId);

                string resultEncrypt = GetConfigValue(KeyEmailPassword, encryptedDefaultValue);
                string result = EncryptionLibrary.EncryptionUtils.Decrypt(resultEncrypt, GeneralConfigAppName, GeneralConfigAppId);

                return result;
            }
            set
            {
                string encryptedValue = EncryptionLibrary.EncryptionUtils.EncryptData(value, GeneralConfigAppName, GeneralConfigAppId);
                SaveConfigValue(KeyEmailPassword, encryptedValue);
            }
        }
    }
}
