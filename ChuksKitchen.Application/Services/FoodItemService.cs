using System.Data;
using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.FoodItemDtos;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Services;

public class FoodItemService : IFoodItemService
{
    private readonly IFoodItemRepository _foodItemRepository;
    private readonly IUserRepository _userRepository;

    public FoodItemService(IFoodItemRepository foodItemRepository, IUserRepository userRepository)
    {
        _foodItemRepository = foodItemRepository;
        _userRepository = userRepository;

    }

    public async Task<BaseResponseModel<FoodItemDto>> AddFoodItemAsync(FoodItemCreateDto request, Guid adminUserId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(adminUserId);
            if (user == null)
            {
                return BaseResponseModel<FoodItemDto>.FailureResponse("User not found.");
            }

            if (user.Role != UserRole.Admin)
            {
                return BaseResponseModel<FoodItemDto>.FailureResponse("Only Admin can add food items.");
            }


            if (string.IsNullOrWhiteSpace(request.Name))
                return BaseResponseModel<FoodItemDto>.FailureResponse("Food name cannot be empty.");

            // Check duplicate
            var existingFood = (await _foodItemRepository.GetAllAsync())
                               .FirstOrDefault(f => f.Name.ToLower() == request.Name.ToLower());
            if (existingFood != null)
            {
                return BaseResponseModel<FoodItemDto>.FailureResponse("Food item with this name already exists.");
            }

            var item = new FoodItem
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
                CreatedBy = adminUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _foodItemRepository.AddAsync(item);

            var itemDto = new FoodItemDto(item.Id, item.Name, item.Price, item.Description, item.IsAvailable);
            return BaseResponseModel<FoodItemDto>.SuccessResponse(itemDto, "Food item created successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<FoodItemDto>.FailureResponse("An error occurred while creating the food item.");
        }
    }

    // Get All Foods
    public async Task<BaseResponseModel<IEnumerable<FoodItemDto>>> GetAllFoodItemsAsync()
    {
        try
        {
            var items = await _foodItemRepository.GetAllAsync();
            var dtos = items.Select(f => new FoodItemDto(f.Id, f.Name, f.Price, f.Description, f.IsAvailable));
            return BaseResponseModel<IEnumerable<FoodItemDto>>.SuccessResponse(dtos, "Food items retrieved successfully.");
        }
        catch (Exception)
        {

            return BaseResponseModel<IEnumerable<FoodItemDto>>.FailureResponse("An error occurred while retrieving food items.");
        }
    }

    // Get Available foods
    public async Task<BaseResponseModel<IEnumerable<FoodItemDto>>> GetAvailableAsync()
    {
        try
        {

            var items = await _foodItemRepository.GetAvailableAsync();
            var dtos = items.Select(f => new FoodItemDto(f.Id, f.Name, f.Price, f.Description, f.IsAvailable));
            return BaseResponseModel<IEnumerable<FoodItemDto>>.SuccessResponse(dtos, "Available food items retrieved successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<IEnumerable<FoodItemDto>>.FailureResponse("An error occurred while retrieving available food items.");
        }
    }

    // Get Food Item by Id
    public async Task<BaseResponseModel<FoodItemDto?>> GetFoodItemByIdAsync(Guid id)
    {
        try
        {
            var item = await _foodItemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return BaseResponseModel<FoodItemDto>.FailureResponse("Food item not found.");
            }

            var dto = new FoodItemDto(item.Id, item.Name, item.Price, item.Description, item.IsAvailable);
            return BaseResponseModel<FoodItemDto>.SuccessResponse(dto, "Food item retrieved successfully.");
        }
        catch (Exception)
        {

            return BaseResponseModel<FoodItemDto>.FailureResponse("An error occurred while retrieving the food item.");
        }
    }


    // Update Food Item Admin Only
    public async Task<BaseResponseModel<FoodItemDto>> UpdateFoodItemAsync(Guid foodId, FoodItemUpdateDto dto, Guid adminUserId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(adminUserId);
            if (user == null || user.Role != UserRole.Admin)
            {
                return BaseResponseModel<FoodItemDto>.FailureResponse("Only admin can update food items.");
            }

            var item = await _foodItemRepository.GetByIdAsync(foodId);
            if (item == null)
            {
                return BaseResponseModel<FoodItemDto>.FailureResponse("Food item not found.");
            }

            item.Name = dto.Name;
            item.Price = dto.Price;
            item.Description = dto.Description;
            item.IsAvailable = dto.IsAvailable;
            item.UpdatedAt = DateTime.UtcNow;

            await _foodItemRepository.UpdateAsync(item);

            var itemDto = new FoodItemDto(item.Id, item.Name, item.Price, item.Description, item.IsAvailable);
            return BaseResponseModel<FoodItemDto>.SuccessResponse(itemDto, "Food item updated successfully.");
        }
        catch (Exception)
        {

            return BaseResponseModel<FoodItemDto>.FailureResponse("An error occurred while updating the food item.");
        }
    }

    public async Task<BaseResponseModel<bool>> DeleteFoodItemAsync(Guid foodId, Guid adminUserId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(adminUserId);
            if (user == null)
            {
                return BaseResponseModel<bool>.FailureResponse("User not found.");
            }

            if (user.Role != UserRole.Admin)
            {
                return BaseResponseModel<bool>.FailureResponse("Only Admin can delete food items.");
            }

            var item = await _foodItemRepository.GetByIdAsync(foodId);
            if (item == null)
            {
                return BaseResponseModel<bool>.FailureResponse("Food item not found.");
            }  

            await _foodItemRepository.DeleteAsync(item);

            return BaseResponseModel<bool>.SuccessResponse(true, "Food item deleted successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<bool>.FailureResponse("An error occurred while deleting the food item.");
        }
    }


}
