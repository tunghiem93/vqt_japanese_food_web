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
                // ===== Catalogs (Top 5)
                model.Catalogs = await _context.Catalogs
                    .AsNoTracking()
                    .Where(x => !x.IsDelete && x.IsActive)
                    .OrderBy(x => x.CreatedAt)
                    .Take(5)
                    .Select(x => new CatalogDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToListAsync();

                // ===== Categories (Top 10)
                model.Categories = await _context.Categorys
                    .AsNoTracking()
                    .Where(x => !x.IsDelete && x.IsActive)
                    .OrderBy(x => x.CreatedAt)
                    .Take(10)
                    .Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CatalogId = x.CatalogId,
                        CatalogName = x.Catalog.Name
                    })
                    .ToListAsync();

                // ===== Product Sales (Top 15)
                model.ProductSales.Products = await _context.Products
                    .AsNoTracking()
                    .Where(p => !p.IsDelete && p.IsActive
                             && p.IsOnSale
                             && p.DiscountId.HasValue
                             && p.DiscountId > 0)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(15)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                        CategoryName = p.Category.Name,
                        Images = p.Images
                            .OrderBy(i => i.SortOrder)
                            .Select(i => new ImageDto
                            {
                                Id = i.Id,
                                Alt = i.Alt,
                                Title = i.Title,
                                ImageUrl = i.ImageUrl,
                                ProductId = i.ProductId
                            }).ToList(),
                        IsActive = p.IsActive,
                        IsDelete = p.IsDelete,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        UpdatedAt = p.UpdatedAt,
                        UpdatedBy = p.UpdatedBy
                    })
                    .ToListAsync();

                // ===== Load ALL data once
                var catalogs = await _context.Catalogs
                    .AsNoTracking()
                    .Where(x => !x.IsDelete && x.IsActive)
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync();

                var categories = await _context.Categorys
                    .AsNoTracking()
                    .Where(x => !x.IsDelete && x.IsActive)
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync();

                var products = await _context.Products
                    .AsNoTracking()
                    .Where(x => !x.IsDelete && x.IsActive)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                        CategoryName = p.Category.Name,
                        CategoryId = p.CategoryId,
                        Images = p.Images
                            .OrderBy(i => i.SortOrder)
                            .Select(i => new ImageDto
                            {
                                Id = i.Id,
                                Alt = i.Alt,
                                Title = i.Title,
                                ImageUrl = i.ImageUrl,
                                ProductId = i.ProductId
                            }).ToList(),
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                model.Products = catalogs.Select(c => new CatalogViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Categories = categories
                        .Where(cat => cat.CatalogId == c.Id)
                        .Select(cat => new CategoryViewModel
                        {
                            Id = cat.Id,
                            Name = cat.Name,
                            Products = products
                                .Where(p => p.CategoryId == cat.Id)
                                .Take(10)
                                .ToList()
                        }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Home Index error");
                throw;
            }

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
