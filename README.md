# **1. System Overview**

## 1.1 Introduction

ChuksKitchen is a RESTful backend API designed to power an online food ordering platform.
The system enables customers to register, verify their accounts using OTP, browse food items,
manage carts, place orders, and participate in a referral system.

The backend follows a layered architecture:

- Domain Layer – Entities and business rules
- Application Layer – Services and interfaces
- Infrastructure Layer – Database and repository  
  implementations
- API Layer – Controllers and HTTP endpoints
  This structure promotes separation of concerns, maintainability, and scalability.

## 1.2 End-to-End System Flow

 From a high-level perspective:
1. A user registers using email or  
   phone.
2. The system generates and sends an
   OTP.
3. The user verifies their account.
4. The user browses available food
   items.
5. The user adds items to their cart.
6. The user places an order.
7. The system stores the order and preserves pricing history.
8. The user can rate food items they have ordered.

## Admins can:

- Create food items
- Update pricing
- Mark items unavailable

## 2. Flow Explanation

### 2.1 User Registration & OTP Verification Flow

### Step-by-Step Flow

1. User submits registration request.
2. System validates:
   Email/phone uniqueness
   Referral code (if provided)

3. A new User record is created with: IsVerified = false
4. A Cart is automatically created for the user.
5. An OTP is generated and stored in UserOtp.
6. OTP is sent via email or phone.
7. User submits OTP for verification.
8. System validates:
   OTP exists
   OTP not expired
   OTP not used
9. If valid:
   User IsVerified = true
   OTP marked as used

## Design Decisions

OTP stored in separate table to allow resend functionality.
Cart created at registration to simplify future cart operations.
Self-referencing relationship used for referral system instead of separate table.

## 2.2 Cart Flow

### Step-by-Step Flow

1. User selects a food item.
2. System validates:
   Food item exists
   Food item is available
3. If CartItem exists → update quantity.
4. If not → create new CartItem.
5. Cart reflects updated state.

## Design Decisions

CartItem used as a junction table to resolve many-to-many relationship.
Quantity stored at CartItem level to allow multiple units.

## 2.3 Order Placement Flow

### Step-by-Step Flow

1. User initiates order placement.
2. System retrieves CartItems.
3. Creates new Order record.
4. For each CartItem:
   Creates OrderItem
   Stores PriceAtOrder
5. Calculates TotalPrice.
6. Order status set to Pending.

## Design Decisions

PriceAtOrder ensures historical pricing integrity.
OrderItem separates transactional data from menu data.
Soft delete prevents accidental data loss.

## 2.4 Rating Flow
### Step-by-Step Flow

1. User submits a rating for a food item.

2. System validates:

- User exists

- User is verified

- Food item exists

- User has previously ordered the food item

- User has not already rated the food item

- Score is between 1 and 5

3. Rating is saved to the database.

### Design Decisions

- Only verified users can rate.

- Users can only rate food items they have ordered.

- Duplicate ratings prevented using a unique constraint (UserId + FoodItemId).

- Score validation enforced at domain level.

- Soft delete supported.

## Edge Case Handling

| Scenario | Handling Strategy |
|----------|-------------------|
| Duplicate email or phone | Registration rejected |
| Invalid referral code | Registration rejected |
| Expired OTP | Verification fails |
| OTP reuse | Blocked via IsUsed flag |
| Food marked unavailable | Cannot add to cart |
| Price change after order | OrderItem keeps original price |
| Multiple OTP requests | Multiple records supported |
| Duplicate rating | Blocked via unique constraint |
| Rating without order | Rejected |
| Data deletion | Soft delete used |

## 4.1 Entity Relationship Diagram

![ERD](./Diagram/erd-correction.png)

### 4.2 Relationship Explanation

User → Cart (1:1)
Each user has exactly one cart.

User → Orders (1:M)
Users can place multiple orders.

User → UserOtp (1:M)
Supports OTP resend and tracking.

User → FoodItems (Admin) (1:M)
Admin users create food items.

Cart → CartItems (1:M)
Cart contains multiple items.

FoodItem → CartItems (1:M)
Food item can appear in multiple carts.

Order → OrderItems (1:M)
Order contains multiple items.

User → Rating (1:M)
A user can submit multiple ratings.

FoodItem → OrderItems (1:M)
Food item can appear in multiple orders.

FoodItem → Rating (1:M)
A food item can receive multiple ratings.

User → User (Self Reference)
Implements referral tracking.

## 5. Assumptions

Authentication middleware (JWT) is not implemented.
Admin accounts are seeded manually.
OTP expiration duration is configurable.
Users must verify before placing orders.
One cart per user.
Payment integration is outside current scope.

## 6. Scalability Considerations (100 → 10,000+ Users)

If the system scales significantly, the following improvements would be implemented:
6.1. Database Optimization
- Index Email, Phone, ReferralCode
- Index foreign keys
- Use database read replicas
- 
6.2 Caching
- Cache frequently accessed FoodItems
- Cache user session data

6.3 Background Processing
- Cleanup expired OTPs
- Archive completed orders
- Use background job processor (e.g., Hangfire)
 
6.4 Architecture Evolution
- Introduce microservices:
- User Service
- Order Service
- Food Service
- Introduce message queue for order processing

6.5 Security Enhancements
- Implement JWT authentication
- Add rate limiting for OTP requests
- Add role-based authorization middleware

## 7. API Endpoints Overview

### Users
POST /api/users/register
POST /api/users/verify-otp

### Food Items
POST /api/fooditems
GET /api/fooditems

### Cart
POST /api/cart/add
GET /api/cart

### Orders
POST /api/orders
GET /api/orders/{id}

### Ratings
POST /api/ratings

## 8. Project Structure
ChuksKitchen
│
├── Domain
├── Application
├── Infrastructure
└── API

## 9. Technology Stack
- .NET (ASP.NET Core)  
- Entity Framework Core
- SQL Database
- Clean Architecture Pattern

## 10. Submission Artifacts
Working API (GitHub Repository)
README Documentation
ERD (PNG)
Flow Diagrams (PNG)
