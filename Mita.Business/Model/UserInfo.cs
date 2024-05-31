using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    using System.Collections.Generic;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        public UserInfo()
        {
            this.UserRights = new HashSet<UserRight>();
        }

        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }

        public virtual ICollection<UserRight> UserRights { get; set; }
    }
}
