using TroyLibrary.Common.DTOs;
using TroyLibrary.Repo.Interfaces;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;

        public BookService(IBookRepo bookRepo)
        {
            _bookRepo = bookRepo;
        }

        public ICollection<BookDTO> GetFeaturedBooks(int? count = 10)
        { 
            var books = this._bookRepo.GetRandomBooks();

            if (count.HasValue)
            {
                books = books.Take(count.Value);
            }

            return books
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    CoverImage = b.CoverImage,
                    Rating = b.Reviews.Average(r => r.Rating),
                    IsAvailable = !b.CheckoutDate.HasValue
                })
                .ToList();
        }

        public ICollection<BookDTO> SearchBooks(string? title = null)
        {
            var books = this._bookRepo.GetBooks();

            if (!string.IsNullOrWhiteSpace(title))
            {
                books = books.Where(b => b.Title.Contains(title));
            }

            return books
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    CoverImage = b.CoverImage,
                    Rating = b.Reviews.Average(r => r.Rating),
                    IsAvailable = !b.CheckoutDate.HasValue
                })
                .ToList();
        }
    }
}
