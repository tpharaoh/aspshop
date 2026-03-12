using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;
using aspshop.Services;

namespace aspshop.Pages.Products;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly CartService _cart;

    public IndexModel(AppDbContext db, CartService cart)
    {
        _db = db;
        _cart = cart;
    }

    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public string? SearchTerm { get; set; }
    public string? CategoryFilter { get; set; }

    public async Task OnGetAsync(string? search, string? category)
    {
        SearchTerm = search;
        CategoryFilter = category;
        Categories = await _db.Categories.ToListAsync();

        var query = _db.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Name.Contains(search) || p.Description.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category.Slug == category);
        }

        Products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId, string? search, string? category)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product != null)
        {
            _cart.AddToCart(product.Id, product.Name, product.Price, product.ImageUrl);
        }
        return RedirectToPage(new { search, category });
    }
}
