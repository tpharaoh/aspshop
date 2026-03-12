using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;

namespace aspshop.Pages.Admin.Categories;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Category> Categories { get; set; } = new();

    public async Task OnGetAsync()
    {
        Categories = await _db.Categories.Include(c => c.Products).OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync(string name, string? description)
    {
        var slug = name.ToLower().Replace(" ", "-");
        _db.Categories.Add(new Category { Name = name, Slug = slug, Description = description ?? "" });
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category != null)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
