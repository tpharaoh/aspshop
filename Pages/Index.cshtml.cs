using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;
using aspshop.Services;

namespace aspshop.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly CartService _cart;

    public IndexModel(AppDbContext db, CartService cart)
    {
        _db = db;
        _cart = cart;
    }

    public List<Category> Categories { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();

    public async Task OnGetAsync()
    {
        Categories = await _db.Categories.ToListAsync();
        FeaturedProducts = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(6)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product != null)
        {
            _cart.AddToCart(product.Id, product.Name, product.Price, product.ImageUrl);
        }
        return RedirectToPage();
    }
}
