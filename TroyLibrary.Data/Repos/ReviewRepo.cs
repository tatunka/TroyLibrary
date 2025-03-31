using System.Data.Entity;
using TroyLibrary.Data.Models;
using TroyLibrary.Data.Repos.Interfaces;

namespace TroyLibrary.Data.Repos
{
    public class ReviewRepo: IReviewRepo
    {
        private readonly TroyLibraryContext _context;

        public ReviewRepo(TroyLibraryContext context)
        {
            _context = context;
        }

        public IQueryable<Review> GetReviews(int bookId)
        {
            return _context.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.TroyLibraryUser);
        }

        public async Task<Review> CreateReview(Review review)
        {
            var r = await _context.Reviews.AddAsync(review);
            _context.SaveChanges();
            return r.Entity;
        }

    }
}
