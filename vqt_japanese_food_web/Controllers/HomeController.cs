using AutoMapper;
using JapaneseFood.Common.Enums;
using JapaneseFood.Entity;
using JapaneseFood.Entity.Product;
using JapaneseFood.Model.Catalog;
using JapaneseFood.Model.Category;
using JapaneseFood.Model.Discount;
using JapaneseFood.Model.Image;
using JapaneseFood.Model.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using vqt_japanese_food_web.Models;

namespace vqt_japanese_food_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public HomeController(DataContext context, IMapper mapper, ILogger<HomeController> logger)
        {
            this._context = context;
            this._mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModels();
            try
            {
                var Catalog = await _context.Catalogs
                    .Where(z => !z.IsDelete && z.IsActive)
                    .OrderBy(z => z.CreatedAt)
                    .Take(5)
                    .Select(z => new CatalogDto
                    {
                        Id = z.Id,
                        Name = z.Name,

                    }).ToListAsync();

                model.Catalogs = Catalog;

                var Category = await _context.Categorys
                    .Where(z => !z.IsDelete && z.IsActive)
                    .OrderBy(o => o.CreatedAt)
                    .Take(10)
                    .Select(z => new CategoryDto
                    {
                        Id = z.Id,
                        Name = z.Name,
                        CatalogId = z.CatalogId,
                        CatalogName = z.Catalog.Name,
                    }).ToListAsync();

                model.Categories = Category;

                var productSales = await _context.Products
                    .Where(z => !z.IsDelete && z.IsActive
                        && z.IsOnSale && z.DiscountId.HasValue 
                        && z.DiscountId.Value > 0)
                    .Select(s => new ProductDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        CategoryName = s.Category.Name,
                        Images = s.Images
                            .OrderBy(i => i.SortOrder)
                            .Select(i => new ImageDto
                            {
                                Id = i.Id,
                                Alt = i.Alt,
                                Title = i.Title,
                                ImageUrl = i.ImageUrl,
                                ProductId = i.ProductId
                            }).ToList(),
                        IsActive = s.IsActive,
                        IsDelete = s.IsDelete,
                        CreatedAt = s.CreatedAt,
                        CreatedBy = s.CreatedBy,
                        UpdatedAt = s.UpdatedAt,
                        UpdatedBy = s.UpdatedBy
                    }).OrderByDescending(z => z.CreatedAt)
                    .Take(15)
                    .ToListAsync();

                model.ProductSales.Products = productSales;
            }
            catch (Exception ex) { }
            return View(model);
        }

        public async Task<ActionResult> Quickview(int q = 0)
        {
            var model = new ProductViewModels();
            try
            {
                var entity = await _context.Products
                    .Where(w => w.Id == q)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return RedirectToAction("Index");
                }

                model.Product = _mapper.Map<ProductEntities, ProductDto>(entity);
                model.Product.Images = _context.Images
                    .Where(w => w.ProductId == q)
                    .OrderBy(o => o.SortOrder)
                    .Select(s => new ImageDto
                    {
                        Id = s.Id,
                        ImageUrl = s.ImageUrl,
                        Alt = s.Alt,
                        Title = s.Title,
                        SortOrder = s.SortOrder,
                        ProductId = s.ProductId
                    }).ToList();

                var discounts = await _context.Discounts
                    .Where(z => !z.IsDelete && z.IsActive)
                    .OrderBy(z => z.CreatedAt)
                    .Select(z => new DiscountDto
                    {
                        Id = z.Id,
                        Name = z.Name,
                        Type = z.Type,
                        TypeName = (z.Type == (int)DiscountTypeEnum.Percentage ? "Percen" : "Fixed"),
                        Value = z.Value,
                    }).ToListAsync();

                model.Discounts = discounts;
            }
            catch (Exception ex) { }
            return PartialView("_PartialProductInfo", model);
        }
    }
}
