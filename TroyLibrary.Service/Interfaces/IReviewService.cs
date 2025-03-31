using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Service.Interfaces
{
    public interface IReviewService
    {
        ICollection<ReviewDTO> GetReviews(int bookId);
        Task<ReviewDTO> CreateReview(string userId, int bookId, int rating, string text);
    }
}
