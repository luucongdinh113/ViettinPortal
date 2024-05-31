using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionLibrary
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ApplicationNameAttribute : Attribute 
    {
        public string ApplicationName { get; set; }

        public ApplicationNameAttribute(string applicationName)
        {
            this.ApplicationName = applicationName;
        }
    }
}
