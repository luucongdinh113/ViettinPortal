using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("StatMonthlyResult")]
    public partial class StatMonthlyResult
    {
        [Key, Column(Order = 0)]
        public Guid SysId { get; set; }
        public string SiteCode { get; set; }
        public string SiteSendInfo { get; set; }
        public DateTime StatMonth { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public long CountTest { get; set; }
        public string UserI { get; set; }
        public DateTime InTime { get; set; }
        public string UserU { get; set; }
        public DateTime UpdateTo { get; set; }
    }
}
