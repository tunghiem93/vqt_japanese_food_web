using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseFood.Entity.Order
{
    [Table("Orders")]
    public class OrderEntities
    {
        public long UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
    }
}
