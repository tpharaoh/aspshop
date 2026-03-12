# EShop Specification

## Overview

A sample e-commerce web application for learning ASP.NET Core with Razor Pages. Sells both physical and digital products with full user authentication, shopping cart, and admin management.

**Purpose**: Learning/Portfolio project to demonstrate ASP.NET + TailwindCSS skills

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| **Framework** | ASP.NET Core 9.0 |
| **Pattern** | Razor Pages |
| **Database** | SQLite + Entity Framework Core |
| **Styling** | TailwindCSS (standalone CLI) |
| **Auth** | ASP.NET Core Identity |
| **Dev Environment** | Docker + Dev Containers |

---

## Features

### Core Features

- [ ] **Product Catalog** - Browse products with images, descriptions, prices
- [ ] **Product Categories** - Organize products (Electronics, Books, Software, etc.)
- [ ] **Search** - Search products by name and description
- [ ] **Shopping Cart** - Add/remove items, update quantities
- [ ] **Mock Checkout** - Simulated payment flow (no real transactions)

### User Features

- [ ] **Registration** - Create account with email/password
- [ ] **Login/Logout** - Secure authentication
- [ ] **User Profile** - View/edit account details
- [ ] **Order History** - View past orders
- [ ] **Saved Addresses** - Store shipping addresses (for physical goods)
- [ ] **Reviews & Ratings** - Rate products 1-5 stars, write reviews

### Admin Features

- [ ] **Admin Dashboard** - Overview of orders, products, users
- [ ] **Product Management** - CRUD operations for products
- [ ] **Category Management** - Create/edit/delete categories
- [ ] **Order Management** - View and update order status

---

## Data Models

### Product
```
- Id (int, PK)
- Name (string)
- Description (string)
- Price (decimal)
- ImageUrl (string)
- CategoryId (int, FK)
- ProductType (enum: Physical, Digital)
- StockQuantity (int, nullable - physical only)
- DownloadUrl (string, nullable - digital only)
- CreatedAt (datetime)
- IsActive (bool)
```

### Category
```
- Id (int, PK)
- Name (string)
- Slug (string)
- Description (string)
```

### User (extends IdentityUser)
```
- FirstName (string)
- LastName (string)
- Addresses (collection)
```

### Address
```
- Id (int, PK)
- UserId (string, FK)
- Street (string)
- City (string)
- State (string)
- PostalCode (string)
- Country (string)
- IsDefault (bool)
```

### Order
```
- Id (int, PK)
- UserId (string, FK)
- OrderDate (datetime)
- Status (enum: Pending, Processing, Shipped, Delivered, Cancelled)
- TotalAmount (decimal)
- ShippingAddressId (int, FK, nullable)
- OrderItems (collection)
```

### OrderItem
```
- Id (int, PK)
- OrderId (int, FK)
- ProductId (int, FK)
- Quantity (int)
- UnitPrice (decimal)
```

### Review
```
- Id (int, PK)
- ProductId (int, FK)
- UserId (string, FK)
- Rating (int, 1-5)
- Title (string)
- Comment (string)
- CreatedAt (datetime)
```

### CartItem (session-based or DB)
```
- Id (int, PK)
- UserId (string, FK, nullable for guests)
- SessionId (string, for guests)
- ProductId (int, FK)
- Quantity (int)
```

---

## Pages Structure

```
/                           - Home (featured products, categories)
/products                   - All products (with search/filter)
/products/{slug}            - Product detail page
/category/{slug}            - Products by category
/cart                       - Shopping cart
/checkout                   - Checkout flow (mock)
/checkout/confirmation      - Order confirmation

/account/register           - Registration
/account/login              - Login
/account/profile            - User profile
/account/orders             - Order history
/account/orders/{id}        - Order detail
/account/addresses          - Manage addresses

/admin                      - Admin dashboard
/admin/products             - Product list
/admin/products/create      - Add product
/admin/products/edit/{id}   - Edit product
/admin/categories           - Category management
/admin/orders               - Order management
```

---

## UI Design

**Theme**: Dark mode with TailwindCSS

**Color Palette**:
- Background: `slate-900`, `slate-800`
- Surface: `slate-800`, `slate-700`
- Primary accent: `indigo-500`, `indigo-400`
- Text: `slate-100`, `slate-300`
- Success: `emerald-500`
- Error: `red-500`

**Components**:
- Responsive navbar with cart icon + count
- Product cards with hover effects
- Star rating display
- Toast notifications for cart actions
- Mobile-friendly sidebar filters

---

## Implementation Phases

### Phase 1: Foundation
1. Project setup (Razor Pages, EF Core, Identity)
2. Database models and migrations
3. TailwindCSS configuration
4. Basic layout with dark theme

### Phase 2: Product Catalog
1. Product and Category models
2. Seed data (sample products)
3. Product listing page with pagination
4. Product detail page
5. Category filtering
6. Search functionality

### Phase 3: Shopping Cart
1. Cart service (session-based)
2. Add to cart functionality
3. Cart page with quantity updates
4. Cart icon with item count

### Phase 4: Authentication
1. ASP.NET Identity setup
2. Register/Login pages
3. User profile page
4. Protected routes

### Phase 5: Checkout & Orders
1. Mock checkout flow
2. Order creation
3. Order history page
4. Order detail page

### Phase 6: Reviews
1. Review model and form
2. Display reviews on product page
3. Average rating calculation

### Phase 7: Admin
1. Admin role and authorization
2. Product CRUD pages
3. Category management
4. Order management dashboard

---

## Seeded Accounts

| Role | Email | Password |
|------|-------|----------|
| **Admin** | `admin@eshop.com` | `Admin123!` |

The admin user is created automatically on first run. Customers register via `/account/register`.

Admin pages (`/admin/*`) require the "Admin" role. Regular users are redirected to login if they try to access them.

---

## Out of Scope (for this version)

- Real payment processing
- Email notifications
- Inventory management alerts
- Wishlists
- Product variants (size, color)
- Coupons/discounts
- Multi-language support
- Advanced analytics

---

## Success Criteria

1. User can browse products, add to cart, and complete mock checkout
2. User can register, login, and view order history
3. Admin can manage products and categories
4. UI is responsive and uses dark mode consistently
5. Code demonstrates clean Razor Pages patterns
6. Project runs entirely in Dev Container
