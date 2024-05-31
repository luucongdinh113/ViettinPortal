using System;

namespace EncryptionLibrary
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ApplicationIdAttribute : Attribute
    {
        public string ApplicationId { get; set; }

        public ApplicationIdAttribute(string applicationId)
        {
            this.ApplicationId = applicationId;
        }
    }
}
