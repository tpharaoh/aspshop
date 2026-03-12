using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspshop.Services;

namespace aspshop.Pages.Cart;

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

    public IActionResult OnPostUpdate(int productId, int quantity)
    {
        _cart.UpdateQuantity(productId, quantity);
        return RedirectToPage();
    }

    public IActionResult OnPostRemove(int productId)
    {
        _cart.RemoveFromCart(productId);
        return RedirectToPage();
    }
}
