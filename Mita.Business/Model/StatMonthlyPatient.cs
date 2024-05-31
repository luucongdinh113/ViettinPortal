using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("StatMonthlyPatient")]
    public partial class StatMonthlyPatient
    {
        [Key, Column(Order = 0)]
        public Guid SysId { get; set; }
        public string SiteCode { get; set; }
        public string SiteSendInfo { get; set; }
        public DateTime StatMonth { get; set; }
        public long CountPatient { get; set; }
        public string UserI { get; set; }
        public DateTime InTime { get; set; }
        public string UserU { get; set; }
        public DateTime UpdateTo { get; set; }
    }
}
