using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mita.Business.Model
{
    [Table("Customers")]
    public partial class Customer
    {
        [Key, Column(Order = 0)]
        public Guid SysId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string UserI { get; set; }
        public DateTime InTime { get; set; }
        public string UserU { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
