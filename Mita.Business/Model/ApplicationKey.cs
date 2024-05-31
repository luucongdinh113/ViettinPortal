using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("ApplicationKey")]
    public partial class ApplicationKey
    {
        [Key, Column(Order = 0)]
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
    }
}
