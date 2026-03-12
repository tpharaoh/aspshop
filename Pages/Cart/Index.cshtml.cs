using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspshop.Services;

namespace aspshop.Pages.Cart;

public class IndexModel : PageModel
{
    private readonly CartService _cart;
    private readonly RecommendationService _recs;

    public IndexModel(CartService cart, RecommendationService recs)
    {
        _cart = cart;
        _recs = recs;
    }

    public List<CartItem> CartItems { get; set; } = new();
    public decimal CartSubtotal { get; set; }
    public decimal CartTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool HasDiscount { get; set; }
    public List<aspshop.Models.Product> Suggestions { get; set; } = new();

    public async Task OnGetAsync()
    {
        CartItems = _cart.GetCart();
        CartSubtotal = _cart.GetCartSubtotal();
        CartTotal = _cart.GetCartTotal();
        DiscountAmount = _cart.GetDiscountAmount();
        HasDiscount = _cart.HasSocialDiscount();

        if (CartItems.Any())
        {
            var cartProductIds = CartItems.Select(ci => ci.ProductId).ToList();
            Suggestions = await _recs.GetCartSuggestions(cartProductIds, 4);
        }
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

    public async Task<IActionResult> OnPostAddSuggestionAsync(int productId)
    {
        var db = HttpContext.RequestServices.GetRequiredService<aspshop.Data.AppDbContext>();
        var product = await db.Products.FindAsync(productId);
        if (product != null)
        {
            _cart.AddToCart(product.Id, product.Name, product.Price, product.ImageUrl);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostApplyDiscount()
    {
        _cart.ApplySocialDiscount();
        return RedirectToPage();
    }
}
