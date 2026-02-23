using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.RatingDto;
using ChuksKitchen.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChuksKitchen.API.Controllers;

[ApiController]
[Route("api/ratings")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    // Create rating
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponseModel<RatingResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<RatingResponse>), 400)]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequest request)
    {
        var result = await _ratingService.CreateRatingAsync(
            request.UserId,
            request.FoodItemId,
            request.Score,
            request.Comment
        );

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}

