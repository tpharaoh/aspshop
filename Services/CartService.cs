using System.Text.Json;

namespace aspshop.Services;

public class CartItem
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class CartService
{
    private const string CartKey = "ShoppingCart";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public List<CartItem> GetCart()
    {
        var json = Session.GetString(CartKey);
        return json == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        Session.SetString(CartKey, JsonSerializer.Serialize(cart));
    }

    public void AddToCart(int productId, string name, decimal price, string imageUrl, int quantity = 1)
    {
        var cart = GetCart();
        var existing = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = productId,
                Name = name,
                Price = price,
                ImageUrl = imageUrl,
                Quantity = quantity
            });
        }
        SaveCart(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
                cart.Remove(item);
            else
                item.Quantity = quantity;
        }
        SaveCart(cart);
    }

    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(c => c.ProductId == productId);
        SaveCart(cart);
    }

    public void ClearCart()
    {
        Session.Remove(CartKey);
        Session.Remove("SocialDiscount");
    }

    public int GetCartCount()
    {
        return GetCart().Sum(c => c.Quantity);
    }

    public decimal GetCartSubtotal()
    {
        return GetCart().Sum(c => c.Price * c.Quantity);
    }

    public decimal GetCartTotal()
    {
        var subtotal = GetCartSubtotal();
        if (HasSocialDiscount())
        {
            return Math.Round(subtotal * 0.9m, 2);
        }
        return subtotal;
    }

    public decimal GetDiscountAmount()
    {
        if (!HasSocialDiscount()) return 0;
        return Math.Round(GetCartSubtotal() * 0.1m, 2);
    }

    public bool HasSocialDiscount()
    {
        return Session.GetString("SocialDiscount") == "true";
    }

    public void ApplySocialDiscount()
    {
        Session.SetString("SocialDiscount", "true");
    }
}
