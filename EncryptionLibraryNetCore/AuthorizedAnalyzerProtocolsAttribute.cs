using System;

namespace EncryptionLibrary
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AuthorizedAnalyzerProtocolsAttribute : Attribute
    {
        public string AuthorizedAnalyzerProtocols { get; set; }

        public AuthorizedAnalyzerProtocolsAttribute(string protocols)
        {
            this.AuthorizedAnalyzerProtocols = protocols;
        }
    }
}
