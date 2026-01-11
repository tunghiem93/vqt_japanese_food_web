using JapaneseFood.Model.Catalog;
using JapaneseFood.Model.Category;
using JapaneseFood.Model.Product;

namespace vqt_japanese_food_web.Models
{
    public class HomeViewModels
    {        
        public List<string> Banners { get; set; } = new List<string>
        {
            "/images/banner1.jpg",
            "/images/banner2.jpg",
            "/images/banner3.jpg"
        };

        public List<CatalogDto> Catalogs { get; set; } = new List<CatalogDto>();
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public ProductSaleViewModel ProductSales { get; set; } = new ProductSaleViewModel();
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string? Sort { get; set; }

        // filter
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }

    public class ProductSaleViewModel
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }

    public class ProductNearViewModel
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }

    public class ProductRateViewModel
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }

    public class ProductFavoriteViewModel
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }   
}
