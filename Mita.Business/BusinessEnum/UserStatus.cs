using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mita.Business.BusinessEnum
{
    public enum UserStatus
    {
        [Description("Khóa")]
        Disable = 0,

        [Description("Đang hoạt động")]
        Enable = 1,

        [Description("Xóa")]
        Remove = 2
    }
}
