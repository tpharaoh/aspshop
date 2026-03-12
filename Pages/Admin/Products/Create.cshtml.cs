using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;

namespace aspshop.Pages.Admin.Products;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    public class InputModel
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public ProductType ProductType { get; set; }
        public int? StockQuantity { get; set; }
        public string ImageUrl { get; set; } = "/images/placeholder.jpg";
    }

    public async Task OnGetAsync()
    {
        await LoadCategories();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return Page();
        }

        var product = new Product
        {
            Name = Input.Name,
            Description = Input.Description,
            Price = Input.Price,
            CategoryId = Input.CategoryId,
            ProductType = Input.ProductType,
            StockQuantity = Input.StockQuantity,
            ImageUrl = Input.ImageUrl
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return RedirectToPage("/Admin/Products/Index");
    }

    private async Task LoadCategories()
    {
        CategoryOptions = await _db.Categories
            .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
            .ToListAsync();
    }
}
