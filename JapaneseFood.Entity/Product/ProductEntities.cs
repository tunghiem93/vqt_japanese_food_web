using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseFood.Entity.Product
{
    [Table("Products")]
    public class ProductEntities : BaseEntities
    {
        public long CategoryId { get; set; }       
        public required string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }            
        public decimal? SalePrice { get; set; }       
        public bool IsOnSale { get; set; }
        public int Quantity { get; set; }   
        public string? Address { get; set; }    
        public double Latitude { get; set; }   
        public double Longitude { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsAvailable { get; set; }
    }
}
