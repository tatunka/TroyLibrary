using Microsoft.AspNetCore.Identity;
using TroyLibrary.Common.DTOs;
using TroyLibrary.Data.Models;
using TroyLibrary.Data.Repos.Interfaces;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;

        public ReviewService(IReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public ICollection<ReviewDTO> GetReviews(int bookId)
        {
            var reviews = _reviewRepo.GetReviews(bookId);

            return reviews.Select(r => 
                new ReviewDTO
                {
                    ReviewId = r.ReviewId,
                    Username = r.TroyLibraryUser.UserName,
                    Text = r.Text,
                    Rating = r.Rating,
                }).ToList();
        }

        public async Task<ReviewDTO> CreateReview(string? userId, int bookId, int rating, string text)
        {
            if (userId == null)
            {
                throw new Exception("User not found");
            }

            var reviews = this._reviewRepo.GetReviews(bookId);
            if (reviews.Any(r => r.TroyLibraryUserId == userId))
            {
                throw new Exception("You have reviewed this book");
            }

            var r = await _reviewRepo.CreateReview(
                new Review
                {
                    BookId = bookId,
                    TroyLibraryUserId = userId,
                    Text = text,
                    Rating = rating,
                });

            return new ReviewDTO
            {
                ReviewId = r.ReviewId,
                Username = r.TroyLibraryUser?.UserName ?? "Unknown User",
                Text = r.Text,
                Rating = r.Rating,
            };
        }
    }
}
