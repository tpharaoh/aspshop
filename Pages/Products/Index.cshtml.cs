using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;

namespace aspshop.Pages.Products;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
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
}
