using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;

namespace aspshop.Pages.Admin.Orders;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Order> Orders { get; set; } = new();

    public async Task OnGetAsync()
    {
        Orders = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _db.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
