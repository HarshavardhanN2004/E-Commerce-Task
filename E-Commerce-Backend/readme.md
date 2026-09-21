#  E-Commerce Backend

A backend REST API for a full-stack E-Commerce application built using **ASP.NET Core Web API, .NET 10, Entity Framework Core, and SQL Server**.

The API provides authentication, product management, category management, shopping cart functionality, order management, and user profile functionality.

---

##  Technologies Used

* ASP.NET Core Web API
* .NET 10
* Entity Framework Core 10
* SQL Server
* JWT Authentication
* BCrypt Password Hashing
* Swagger
* C#
* Repository Pattern
* Service Layer Architecture

---

##  Architecture

The backend follows a layered architecture:

```text
Client / React Frontend
        ↓
    Controllers
        ↓
      Services
        ↓
    Repositories
        ↓
Entity Framework Core
        ↓
    SQL Server
```


##  Database

The application uses **SQL Server** with Entity Framework Core Code First.

### Main Tables

```text
Users
Categories
Products
Cart
CartItems
Orders
OrderItems
```

### Entity Relationships

```text
User
 │
 ├── Cart
 │     └── CartItems
 │             └── Product
 │
 └── Orders
        └── OrderItems
                └── Product

Category
    │
    └── Products
```

---

##  Authentication

The API uses **JWT Bearer Authentication**.

### Login Flow

```text
Login Request
     ↓
Validate Email
     ↓
Verify Password using BCrypt
     ↓
Generate JWT Token
     ↓
Return Token + User Information
```

The JWT token is required for protected endpoints.

The application supports two roles:

* `Admin`
* `User`

---

##  Main Features

### Authentication

* User registration
* User login
* BCrypt password hashing
* JWT token generation
* Role-based authorization

### Products

* Create product
* View products
* View product by ID
* Update product
* Delete product
* Manage product stock
* Assign products to categories

### Categories

* Create category
* View categories
* Update category
* Delete category

### Cart

* Add product to cart
* Update product quantity
* Remove cart item
* Clear cart
* Calculate cart totals

### Orders

* Create order
* View user orders
* View order details
* Manage order status
* Store shipping information
* Calculate order totals

### User Profile

* Retrieve authenticated user information

---

##  API Modules

```text
Authentication
    ├── Register
    └── Login

Products
    ├── Create
    ├── Get
    ├── Update
    └── Delete

Categories
    ├── Create
    ├── Get
    ├── Update
    └── Delete

Cart
    ├── Get Cart
    ├── Add Item
    ├── Update Quantity
    ├── Remove Item
    └── Clear Cart

Orders
    ├── Create Order
    ├── Get Orders
    ├── Get Order Details
    └── Update Order Status

Users
    └── Get Profile
```


##  API Testing

The API can be tested using:

* Swagger
* React frontend

Swagger provides an interactive interface for testing the API endpoints.

---

##  Security

The backend implements:

* JWT authentication
* BCrypt password hashing
* Role-based authorization
* Protected API endpoints
* Server-side validation
* Entity Framework relationships
* DTO-based API communication

Sensitive information such as database credentials and JWT secret keys should be stored securely and not committed to GitHub.

---

##  Request Flow

Example product request:

```text
React Frontend
      ↓
HTTP Request
      ↓
ProductController
      ↓
ProductService
      ↓
ProductRepository
      ↓
Entity Framework Core
      ↓
SQL Server
      ↓
Response
      ↓
React Frontend
```

---

##  Future Enhancements

* Online payment integration
* Product reviews and ratings
* Wishlist
* Coupon and discount management
* Email notifications
* Advanced search and filtering
* Admin analytics
* Cloud deployment

