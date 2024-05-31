using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mita.Business.BusinessServices
{
    public class DatabaseCommand<InVal, ReturnVal>
    {
        public InVal CallingInfo { get; set; }
        public ReturnVal ReturnValue { get; set; }
    }
}
