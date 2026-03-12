using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspshop.Data;
using aspshop.Models;
using aspshop.Services;

namespace aspshop.Pages.Checkout;

public class IndexModel : PageModel
{
    private readonly CartService _cart;
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(CartService cart, AppDbContext db, UserManager<ApplicationUser> userManager)
    {
        _cart = cart;
        _db = db;
        _userManager = userManager;
    }

    public List<Services.CartItem> CartItems { get; set; } = new();
    public decimal CartSubtotal { get; set; }
    public decimal CartTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool HasDiscount { get; set; }

    public void OnGet()
    {
        CartItems = _cart.GetCart();
        CartSubtotal = _cart.GetCartSubtotal();
        CartTotal = _cart.GetCartTotal();
        DiscountAmount = _cart.GetDiscountAmount();
        HasDiscount = _cart.HasSocialDiscount();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var cartItems = _cart.GetCart();
        if (!cartItems.Any())
            return RedirectToPage("/Cart/Index");

        var userId = _userManager.GetUserId(User);

        var order = new Order
        {
            UserId = userId ?? "guest",
            TotalAmount = _cart.GetCartTotal(),
            OrderItems = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Price
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        _cart.ClearCart();
        return RedirectToPage("/Checkout/Confirmation", new { orderId = order.Id });
    }
}
