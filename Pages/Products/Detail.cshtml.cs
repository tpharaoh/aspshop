using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspshop.Data;
using aspshop.Models;
using aspshop.Services;

namespace aspshop.Pages.Products;

public class DetailModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly CartService _cart;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RecommendationService _recs;

    public DetailModel(AppDbContext db, CartService cart, UserManager<ApplicationUser> userManager, RecommendationService recs)
    {
        _db = db;
        _cart = cart;
        _userManager = userManager;
        _recs = recs;
    }

    public Product? Product { get; set; }
    public double AverageRating { get; set; }
    public List<Product> FrequentlyBoughtTogether { get; set; } = new();
    public List<Product> YouMightAlsoLike { get; set; } = new();

    [BindProperty]
    public ReviewInput Review { get; set; } = new();

    public class ReviewInput
    {
        [Required, Range(1, 5)] public int Rating { get; set; } = 5;
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Comment { get; set; } = string.Empty;
    }

    public async Task OnGetAsync(int id)
    {
        await LoadProduct(id);
        if (Product != null)
        {
            FrequentlyBoughtTogether = await _recs.GetFrequentlyBoughtTogether(id, 4);
            YouMightAlsoLike = await _recs.GetSameCategory(id, 4);
        }
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product != null)
        {
            _cart.AddToCart(product.Id, product.Name, product.Price, product.ImageUrl);
        }
        return RedirectToPage("/Cart/Index");
    }

    public async Task<IActionResult> OnPostReviewAsync(int id)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return RedirectToPage("/Account/Login");

        if (!ModelState.IsValid)
        {
            await LoadProduct(id);
            return Page();
        }

        var userId = _userManager.GetUserId(User)!;

        var review = new Models.Review
        {
            ProductId = id,
            UserId = userId,
            Rating = Review.Rating,
            Title = Review.Title,
            Comment = Review.Comment
        };

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        return RedirectToPage(new { id });
    }

    private async Task LoadProduct(int id)
    {
        Product = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (Product?.Reviews.Any() == true)
        {
            AverageRating = Product.Reviews.Average(r => r.Rating);
        }
    }
}
