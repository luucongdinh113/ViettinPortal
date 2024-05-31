using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("NodeError")]
    public partial class NodeError
    {
        [Key, Column(Order = 0)]
        public Guid SysId { get; set; }
        public string HostName { get; set; }
        public string SiteName { get; set; }
        public DateTime ErrorTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
