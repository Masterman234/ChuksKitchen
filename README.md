# **1. System Overview**

## 1.1 Introduction

ChuksKitchen is a RESTful backend API designed to power an online food ordering platform.
The system enables customers to register, verify their accounts using OTP, browse food items,
manage carts, place orders, and participate in a referral system.

## The backend follows a layered architecture:

- Domain Layer – Entities and business rules
- Application Layer – Services and interfaces
- Infrastructure Layer – Database and repository  
  implementations
- API Layer – Controllers and HTTP endpoints
  This structure promotes separation of concerns, maintainability, and scalability.

## 1.2 End-to-End System Flow

1. From a high-level perspective:
2. A user registers using email or  
   phone.
3. The system generates and sends an
   OTP.
4. The user verifies their account.
5. The user browses available food
   items.
6. The user adds items to their cart.
7. The user places an order.
   The system stores the order and preserves pricing history.

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

## 3. Edge Case Handling

Scenario Handling Strategy
Duplicate email or phone - Registration rejected
Invalid referral code - Registration rejected
Expired OTP - Verification fails
OTP reuse - Blocked via IsUsed flag
Food marked unavailable - Cannot add to cart
Price change after order - OrderItem keeps original price
Multiple OTP requests - Multiple records supported
Data deletion - Soft delete used

## 4.1 Entity Relationship Diagram

![alt text](ERD.png)

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

FoodItem → OrderItems (1:M)
Food item can appear in multiple orders.

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
Index Email, Phone, ReferralCode
Index foreign keys
Use database read replicas
6.2 Caching
Cache frequently accessed FoodItems
Cache user session data
6.3 Background Processing
Cleanup expired OTPs
Archive completed orders
Use background job processor (e.g., Hangfire)
6.4 Architecture Evolution
Introduce microservices:
User Service
Order Service
Food Service
Introduce message queue for order processing
6.5 Security Enhancements
Implement JWT authentication
Add rate limiting for OTP requests
Add role-based authorization middleware

## 7. Technology Stack

.NET (ASP.NET Core)  
 Entity Framework Core
SQL Database
Clean Architecture Pattern

## 8. Submission Artifacts

Working API (GitHub Repository)
README Documentation
ERD (PNG)
Flow Diagrams (PNG)
