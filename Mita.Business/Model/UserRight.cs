using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("UserRights")]
    public partial class UserRight
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [Key, Column(Order = 1)]
        public string RightCode { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
    }
}
