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
| **Styling** | TailwindCSS v4 (standalone CLI) |
| **Auth** | ASP.NET Core Identity |
| **Dev Environment** | Docker + Dev Containers |

---

## Features

### Core Features

- [x] **Product Catalog** - Browse products with descriptions, prices
- [x] **Product Categories** - Organize products (Electronics, Books, Software, Clothing)
- [x] **Search** - Search products by name and description
- [x] **Category Filter** - Filter products by category on `/products`
- [x] **Shopping Cart** - Session-based; add/remove items, update quantities, quick-add from listings
- [x] **Mock Checkout** - Simulated payment flow (no real transactions)

### User Features

- [x] **Registration** - Create account with email/password
- [x] **Login/Logout** - Secure authentication with ASP.NET Identity
- [x] **User Profile** - View/edit first and last name
- [x] **Order History** - View past orders with items and totals
- [ ] **Saved Addresses** - Store shipping addresses (model exists, no UI)
- [x] **Reviews & Ratings** - Rate products 1-5 stars, write reviews (requires login)

### Admin Features

- [x] **Admin Dashboard** - Overview counts: products, categories, orders, users
- [x] **Product Management** - Create, edit, delete products
- [x] **Category Management** - Create and delete categories
- [x] **Order Management** - View orders and update status
- [x] **Role-Based Access** - Admin pages require "Admin" role; nav link hidden from non-admins

---

## Seeded Accounts

| Role | Email | Password |
|------|-------|----------|
| **Admin** | `admin@eshop.com` | `Admin123!` |

The admin user is created automatically on first run. Customers register via `/account/register`.

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

### Cart (session-based, not in DB)
```
Stored as JSON in browser session:
- ProductId (int)
- Name (string)
- Price (decimal)
- ImageUrl (string)
- Quantity (int)
```

---

## Pages Structure

```
Built:
/                           - Home (hero, categories, featured products with quick-add)
/products                   - All products (search, category filter, quick-add)
/products/{id}              - Product detail (reviews display, add to cart)
/cart                       - Shopping cart (quantity +/-, remove)
/checkout                   - Mock checkout (shipping form, fake payment)
/checkout/confirmation      - Order confirmed with random order #

/account/register           - Registration
/account/login              - Login
/account/logout             - Logout (POST only)
/account/profile            - Edit name (requires login)
/account/orders             - Order history (requires login)

/admin                      - Dashboard with stats (requires Admin role)
/admin/products             - Product list + delete
/admin/products/create      - Add product form
/admin/products/edit/{id}   - Edit product form
/admin/categories           - Add/delete categories
/admin/orders               - View orders, update status

Not yet built:
/account/addresses          - Manage saved addresses
/account/orders/{id}        - Individual order detail
```

---

## UI Design

**Theme**: Dark mode with TailwindCSS v4

**Color Palette**:
- Background: `slate-900`, `slate-800`
- Surface: `slate-800`, `slate-700`
- Primary accent: `indigo-500`, `indigo-400`
- Text: `slate-100`, `slate-300`
- Success: `emerald-500`
- Error: `red-500`

**Built Components**:
- Responsive navbar with cart icon + item count badge
- Product cards with hover border effect + quick-add buttons
- Star rating display on product detail
- Auth-aware nav (Login/Register vs Profile/Orders/Logout)
- Admin link visible only to Admin role

**Not Yet Built**:
- Toast notifications for cart actions
- Mobile hamburger menu
- Pagination on product listing
- Product image upload (currently placeholder SVGs)

---

## Showcase Features

These features go beyond a typical sample project to demonstrate real-world patterns.

### 1. Social Share Discount
- [ ] Share button on cart page - "Share for 10% off!"
- [ ] Simulated social share flow (opens mock share dialog)
- [ ] On "confirmation", applies a 10% discount code to the cart
- [ ] Discount shown as line item in cart and checkout
- [ ] One-time use per session (can't stack)
- [ ] Visual badge on cart showing discount is active

### 2. AI Product Recommendations
- [ ] "Frequently Bought Together" section on product detail page (products from same orders)
- [ ] "You Might Also Like" section (same category, different products)
- [ ] "Complete Your Setup" suggestions on cart page based on what's in cart
- [ ] Falls back gracefully when not enough data (shows category matches)

### 3. Real-Time Stock Indicator (SignalR)
- [ ] Live "X people viewing this" counter on product detail
- [ ] Stock level badge updates in real-time (e.g., "Only 3 left!")
- [ ] Simulated viewer count that fluctuates
- [ ] SignalR hub for pushing updates to connected clients

### 4. Dark/Light Theme Toggle
- [ ] Toggle button in navbar
- [ ] Persisted in localStorage (survives page reload)
- [ ] Light theme uses clean white/gray palette
- [ ] Smooth CSS transition between themes
- [ ] Respects system preference on first visit (`prefers-color-scheme`)

### 5. Multi-Language Support (i18n)
- [ ] Language switcher in navbar (English, Spanish, French)
- [ ] Translates UI labels, buttons, nav items
- [ ] Product data stays in English (content translation out of scope)
- [ ] Persisted in cookie/localStorage
- [ ] Uses ASP.NET Core localization middleware

---

## Out of Scope (for this version)

- Real payment processing
- Email notifications
- Inventory management alerts
- Wishlists
- Product variants (size, color)
- Advanced analytics
- Product content translation (UI labels only for i18n)

---

## Success Criteria

1. ~~User can browse products, add to cart, and complete mock checkout~~
2. ~~User can register, login, and view order history~~
3. ~~Admin can manage products and categories~~
4. UI is responsive and uses dark mode consistently
5. Code demonstrates clean Razor Pages patterns
6. ~~Project runs entirely in Dev Container~~
7. Showcase features demonstrate portfolio-level engineering
