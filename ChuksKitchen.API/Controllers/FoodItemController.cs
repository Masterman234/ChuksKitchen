using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.FoodItemDtos;
using ChuksKitchen.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChuksKitchen.API.Controllers;

[ApiController]
[Route("api/foods")]
public class FoodItemController : ControllerBase
{
    private readonly IFoodItemService _foodItemService;

    public FoodItemController(IFoodItemService foodItemService)
    {
        _foodItemService = foodItemService;
    }


    // GET: api/foods
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponseModel<IEnumerable<FoodItemDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _foodItemService.GetAllFoodItemsAsync();
        return Ok(result);
    }

    // GET: api/foods/available
    [HttpGet("available")]
    [ProducesResponseType(typeof(BaseResponseModel<IEnumerable<FoodItemDto>>), 200)]
    public async Task<IActionResult> GetAvailable()
    {
        var result = await _foodItemService.GetAvailableAsync();
        return Ok(result);
    }

    // GET: api/foods/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<FoodItemDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<FoodItemDto>), 404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _foodItemService.GetFoodItemByIdAsync(id);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);

    }

    // POST: api/foods
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponseModel<FoodItemDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<FoodItemDto>), 400)]
    public async Task<IActionResult> Create([FromForm] FoodItemCreateDto dto, [FromQuery] Guid adminUserId)
    {
        var result = await _foodItemService.AddFoodItemAsync(dto, adminUserId);
        if (result.Success)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);

        }
        return BadRequest(result);
    }

    // PUT: api/foods/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<FoodItemDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<FoodItemDto>), 400)]
    public async Task<IActionResult> Update(Guid id, [FromForm] FoodItemUpdateDto dto, [FromQuery] Guid adminUserId)
    {
        var result = await _foodItemService.UpdateFoodItemAsync(id, dto, adminUserId);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }


    // DELETE: api/foods/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 400)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid adminUserId)
    {
        var result = await _foodItemService.DeleteFoodItemAsync(id, adminUserId);
        if (!result.Success)
        {
            return NotFound(result);
        } 

        return Ok(result);

    }



}
