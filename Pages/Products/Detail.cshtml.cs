using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;
using aspshop.Services;

namespace aspshop.Pages.Products;

public class DetailModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly CartService _cart;

    public DetailModel(AppDbContext db, CartService cart)
    {
        _db = db;
        _cart = cart;
    }

    public Product? Product { get; set; }

    public async Task OnGetAsync(int id)
    {
        Product = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product != null)
        {
            _cart.AddToCart(product.Id, product.Name, product.Price, product.ImageUrl);
        }
        return RedirectToPage("/Cart/Index");
    }
}
