using System.ComponentModel;

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
