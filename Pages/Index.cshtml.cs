using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;

namespace aspshop.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
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
}
