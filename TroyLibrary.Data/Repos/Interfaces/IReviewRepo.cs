using TroyLibrary.Data.Models;

namespace TroyLibrary.Data.Repos.Interfaces
{
    public interface IReviewRepo
    {
        IQueryable<Review> GetReviews(int bookId);
        Task<Review> CreateReview(Review review);
    }
}
