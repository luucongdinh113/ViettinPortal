using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionLibrary
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class CustomerIdAttribute : Attribute 
    {
        public string CustomerId { get; set; }

        public CustomerIdAttribute(string customerId)
        {
            this.CustomerId = customerId;
        }
    }
}
