using Mita.Business.BusinessEnum;
using System.Collections.Generic;

namespace Mita.Business.BusinessObjects
{
    public class LoginUserInfo
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public UserStatus Status { get; set; }

        public List<UserRightCode> Rights { get; set; }

        public LoginUserInfo()
        {
            Rights = new List<UserRightCode>();
        }

        public bool IsAuthorize(UserRightCode rightCode)
        {
            return Rights.Contains(rightCode);
        }
    }
}
