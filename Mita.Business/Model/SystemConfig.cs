using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("Config")]
    public partial class SystemConfig
    {
        [Key, Column(Order = 0)]
        public string ConfigID { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
    }
}
