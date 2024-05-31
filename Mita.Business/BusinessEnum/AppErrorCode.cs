using System.ComponentModel;

namespace Mita.Business.BusinessEnum
{
    public enum AppErrorCode
    {
        [Description("UserId đã tồn tại, vui lòng chọn user khác.")]
        UserIdExists,

        [Description("UserId không tồn tại.")]
        UserIdNotExists,

        [Description("User hoặc Mật khẩu không chính xác.")]
        PasswordNotMatch,

        [Description("User không active.")]
        UserNotActive,

        [Description("Lỗi trong hệ thống.")]
        InternalErrorException,

        [Description("Mật khẩu xác nhận không khớp.")]
        VerifyPasswordNotMatch,

        [Description("Mật khẩu cũ không đúng.")]
        OldPasswordNotMatch,

        //[Description("User này không được phép truy cập.")]
        //ApplicationExists,

        [Description("Application Key không tồn tại")]
        ApplicationNotExists,

        [Description("Mã hệ thống khách hàng đã tồn tại.")]
        CustomerSysIdExists,

        [Description("Mã khách hàng đã tồn tại.")]
        CustomerIdExists,
    }
}
