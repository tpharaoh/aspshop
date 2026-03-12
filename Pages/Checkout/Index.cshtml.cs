using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspshop.Services;

namespace aspshop.Pages.Checkout;

public class IndexModel : PageModel
{
    private readonly CartService _cart;

    public IndexModel(CartService cart)
    {
        _cart = cart;
    }

    public List<CartItem> CartItems { get; set; } = new();
    public decimal CartTotal { get; set; }

    public void OnGet()
    {
        CartItems = _cart.GetCart();
        CartTotal = _cart.GetCartTotal();
    }

    public IActionResult OnPost()
    {
        var orderId = Random.Shared.Next(10000, 99999);
        _cart.ClearCart();
        return RedirectToPage("/Checkout/Confirmation", new { orderId });
    }
}
