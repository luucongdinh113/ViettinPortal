using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("NodeMonitor")]
    public partial class NodeMonitor
    {
        [Key, Column(Order = 0)]
        public Guid SysId { get; set; }
        public string HostName { get; set; }
        public string SiteName { get; set; }
        public DateTime CheckTime { get; set; }
        public string SiteCode { get; set; }
    }
}
