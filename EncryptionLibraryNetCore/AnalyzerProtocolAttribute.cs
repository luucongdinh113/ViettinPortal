using System;

namespace EncryptionLibrary
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AnalyzerProtocolsAttribute : Attribute
    {
        public string Protocols { get; set; }

        public AnalyzerProtocolsAttribute(string protocols)
        {
            this.Protocols = protocols;
        }
    }
}
