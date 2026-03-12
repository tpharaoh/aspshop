using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using aspshop.Models;

namespace aspshop.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Order>()
            .HasOne(o => o.ShippingAddress)
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Slug = "electronics", Description = "Gadgets and devices" },
            new Category { Id = 2, Name = "Books", Slug = "books", Description = "Physical and digital books" },
            new Category { Id = 3, Name = "Software", Slug = "software", Description = "Applications and tools" },
            new Category { Id = 4, Name = "Clothing", Slug = "clothing", Description = "Apparel and accessories" }
        );

        // Seed products
        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Wireless Headphones", Description = "Premium noise-cancelling wireless headphones with 30-hour battery life.", Price = 79.99m, ImageUrl = "/images/headphones.jpg", CategoryId = 1, ProductType = ProductType.Physical, StockQuantity = 50, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "C# in Depth", Description = "Comprehensive guide to C# programming by Jon Skeet.", Price = 39.99m, ImageUrl = "/images/csharp-book.jpg", CategoryId = 2, ProductType = ProductType.Physical, StockQuantity = 100, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "Visual Studio Pro License", Description = "One-year license for Visual Studio Professional.", Price = 149.99m, ImageUrl = "/images/vs-pro.jpg", CategoryId = 3, ProductType = ProductType.Digital, DownloadUrl = "/downloads/vs-pro", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "Mechanical Keyboard", Description = "RGB mechanical keyboard with Cherry MX switches.", Price = 129.99m, ImageUrl = "/images/keyboard.jpg", CategoryId = 1, ProductType = ProductType.Physical, StockQuantity = 30, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5, Name = "ASP.NET Core in Action", Description = "Learn to build web applications with ASP.NET Core.", Price = 44.99m, ImageUrl = "/images/aspnet-book.jpg", CategoryId = 2, ProductType = ProductType.Physical, StockQuantity = 75, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 6, Name = "JetBrains Rider License", Description = "One-year license for JetBrains Rider IDE.", Price = 199.99m, ImageUrl = "/images/rider.jpg", CategoryId = 3, ProductType = ProductType.Digital, DownloadUrl = "/downloads/rider", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
