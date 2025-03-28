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

        public async Task<BookDetailDTO> GetBook(int bookId)
        {
            var book = await this._bookRepo.GetBook(bookId);

            return new BookDetailDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CoverImage = book.CoverImage,
                Rating = book.Reviews?.Average(r => r.Rating),
                IsAvailable = !book.CheckoutDate.HasValue,
                PublicationDate = book.PublicationDate,
                ISBN = book.ISBN,
                PageCount = book.PageCount,
                Publisher = book.Publisher,
                Reviews = book.Reviews?.Select(r => new ReviewDTO
                {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    Text = r.Text,
                    Username = r.TroyLibraryUser?.UserName,
                }).ToList()
            };
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
