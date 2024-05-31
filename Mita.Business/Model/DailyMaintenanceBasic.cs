using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("DailyMaintenanceBasic")]
    public partial class DailyMaintenanceBasic
    {
        [Key, Column(Order = 0)]
        public Guid SysId { get; set; }
        public string SiteCode { get; set; }
        public string SiteSendInfo { get; set; }
        public DateTime CheckDate { get; set; }
        public long CountPatient { get; set; }
        public long CountResult { get; set; }
        public DateTime MinDateIN { get; set; }
        public DateTime MaxDateIN { get; set; }
    }
}
