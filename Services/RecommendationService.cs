using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;

namespace aspshop.Services;

public class RecommendationService
{
    private readonly AppDbContext _db;

    public RecommendationService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Products frequently bought together (found in the same orders).
    /// Falls back to same-category products if not enough order data.
    /// </summary>
    public async Task<List<Product>> GetFrequentlyBoughtTogether(int productId, int count = 4)
    {
        // Find other products that appear in the same orders as this product
        var coProducts = await _db.OrderItems
            .Where(oi => oi.OrderId != 0 &&
                _db.OrderItems.Any(other => other.OrderId == oi.OrderId && other.ProductId == productId) &&
                oi.ProductId != productId)
            .GroupBy(oi => oi.ProductId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToListAsync();

        if (coProducts.Count >= count)
        {
            return await _db.Products
                .Include(p => p.Category)
                .Where(p => coProducts.Contains(p.Id) && p.IsActive)
                .ToListAsync();
        }

        // Fallback: same category products
        return await GetSameCategory(productId, count);
    }

    /// <summary>
    /// Products from the same category ("You Might Also Like")
    /// </summary>
    public async Task<List<Product>> GetSameCategory(int productId, int count = 4)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null) return new();

        var candidates = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == product.CategoryId && p.Id != productId && p.IsActive)
            .ToListAsync();

        return candidates.OrderBy(_ => Random.Shared.Next()).Take(count).ToList();
    }

    /// <summary>
    /// Suggest products based on what's in the cart ("Complete Your Setup")
    /// </summary>
    public async Task<List<Product>> GetCartSuggestions(List<int> cartProductIds, int count = 4)
    {
        if (!cartProductIds.Any()) return new();

        // Get categories of items in cart
        var cartCategoryIds = await _db.Products
            .Where(p => cartProductIds.Contains(p.Id))
            .Select(p => p.CategoryId)
            .Distinct()
            .ToListAsync();

        // Find products from related orders first
        var orderSuggestions = await _db.OrderItems
            .Where(oi =>
                _db.OrderItems.Any(other => other.OrderId == oi.OrderId && cartProductIds.Contains(other.ProductId)) &&
                !cartProductIds.Contains(oi.ProductId))
            .GroupBy(oi => oi.ProductId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToListAsync();

        if (orderSuggestions.Count >= count)
        {
            return await _db.Products
                .Include(p => p.Category)
                .Where(p => orderSuggestions.Contains(p.Id) && p.IsActive)
                .ToListAsync();
        }

        // Fallback: products from same categories not in cart
        var fallback = await _db.Products
            .Include(p => p.Category)
            .Where(p => cartCategoryIds.Contains(p.CategoryId) && !cartProductIds.Contains(p.Id) && p.IsActive)
            .ToListAsync();

        return fallback.OrderBy(_ => Random.Shared.Next()).Take(count).ToList();
    }
}
