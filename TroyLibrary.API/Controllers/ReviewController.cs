using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TroyLibrary.Common.Models.Review;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet]
        public GetReviewsResponse GetReviews([FromQuery] int bookId)
        {
            return new GetReviewsResponse
            {
                Reviews = _reviewService.GetReviews(bookId),
            };
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<GetReviewsResponse> CreateReview([FromBody] CreateReviewRequest review)
        {
            var calims = User.Claims;

            return new GetReviewsResponse
            {
                Reviews = [await _reviewService.CreateReview(
                    User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value, 
                    review.BookId, 
                    review.Rating, 
                    review.Text)],
            };
        }
    }
}
