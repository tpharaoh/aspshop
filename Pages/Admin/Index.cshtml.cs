using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;

namespace aspshop.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public int ProductCount { get; set; }
    public int CategoryCount { get; set; }
    public int OrderCount { get; set; }
    public int UserCount { get; set; }

    public async Task OnGetAsync()
    {
        ProductCount = await _db.Products.CountAsync();
        CategoryCount = await _db.Categories.CountAsync();
        OrderCount = await _db.Orders.CountAsync();
        UserCount = await _db.Users.CountAsync();
    }
}
