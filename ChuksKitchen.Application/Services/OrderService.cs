using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.OrderDtos;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _userRepository = userRepository;
    }


    public async Task<BaseResponseModel<OrderDto>> PlaceOrderAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BaseResponseModel<OrderDto>.FailureResponse("User not found");
            }

            var cart = await _cartRepository.GetCartWithItemsAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                return BaseResponseModel<OrderDto>.FailureResponse("Cart is empty");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalPrice = 0,
                Items = new List<OrderItem>()
            };

            decimal totalPrice = 0;
            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    FoodItemId = cartItem.FoodItemId,
                    Quantity = cartItem.Quantity,
                    PriceAtOrder = cartItem.FoodItem.Price,
                    OrderId = order.Id,
                };

                totalPrice += cartItem.FoodItem.Price * cartItem.Quantity;
                order.Items.Add(orderItem);
            }

            order.TotalPrice = totalPrice;

            await _orderRepository.AddAsync(order);

            //Clear Cart After Order
            await _cartRepository.ClearCartAsync(cart);

            var orderDto = new OrderDto(
                order.Id,
                order.UserId,
                order.TotalPrice,
                order.Status,
                order.Items.Select(x =>
                    new OrderItemDto(
                        x.FoodItemId,
                        x.FoodItem?.Name ?? string.Empty,
                        x.PriceAtOrder,
                        x.Quantity
                    )
                ).ToList()
            );

            return BaseResponseModel<OrderDto>.SuccessResponse(orderDto, "Order created successfully");
        }
        catch (Exception ex)
        {

            return BaseResponseModel<OrderDto>
                .FailureResponse($"Error creating order: {ex.Message}");
        }
    }

    public async Task<BaseResponseModel<OrderDto>> GetOrderByIdAsync(Guid orderId)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);
            if (order == null)
            {
                return BaseResponseModel<OrderDto>.FailureResponse("Order not found");
            }

            var dto = MapToDto(order);

            return BaseResponseModel<OrderDto>.SuccessResponse(dto, "Order retrieved successfully");
        }
        catch (Exception)
        {

            return BaseResponseModel<OrderDto>.FailureResponse("An error occured while retrieving order");
        }
    }

    public async Task<BaseResponseModel<List<OrderDto>>> GetUserOrdersAsync(Guid userId)
    {
        try
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            var dtos = orders.Select(MapToDto).ToList();

            return BaseResponseModel<List<OrderDto>>.SuccessResponse(dtos, "Orders retrieved successfully");
        }
        catch (Exception)
        {

            return BaseResponseModel<List<OrderDto>>.FailureResponse("Error retrieving orders");
        }
    }

    public async Task<BaseResponseModel<bool>> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                return BaseResponseModel<bool>.FailureResponse("Order not found");
            }
            if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
            {
                return BaseResponseModel<bool>.FailureResponse("Order can no longer be modified");
            }

            order.Status = status;

            await _orderRepository.Update(order);

            return BaseResponseModel<bool>.SuccessResponse(true, "Order status updated");
        }
        catch (Exception)
        {

            return BaseResponseModel<bool>.FailureResponse("An error occured while updating user");
        }
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto(
            order.Id,
            order.UserId,
            order.TotalPrice,
            order.Status,
            order.Items.Select(i =>
                new OrderItemDto(
                    i.FoodItemId,
                    i.FoodItem?.Name ?? string.Empty,
                    i.PriceAtOrder,
                    i.Quantity
                )
            ).ToList()
        );
    }
}
