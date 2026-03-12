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
public class EditModel : PageModel
{
    private readonly AppDbContext _db;

    public EditModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    public class InputModel
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public ProductType ProductType { get; set; }
        public int? StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        Input = new InputModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            ProductType = product.ProductType,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive
        };

        await LoadCategories();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return Page();
        }

        var product = await _db.Products.FindAsync(Input.Id);
        if (product == null) return NotFound();

        product.Name = Input.Name;
        product.Description = Input.Description;
        product.Price = Input.Price;
        product.CategoryId = Input.CategoryId;
        product.ProductType = Input.ProductType;
        product.StockQuantity = Input.StockQuantity;
        product.ImageUrl = Input.ImageUrl;
        product.IsActive = Input.IsActive;

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
