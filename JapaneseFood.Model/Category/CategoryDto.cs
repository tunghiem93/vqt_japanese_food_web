using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseFood.Model.Category
{
    public class CategoryDto : BaseDto
    {
        public required string Name { get; set; }
        public long CatalogId { get; set; }
    }
}
