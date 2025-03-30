using TroyLibrary.Common;
using TroyLibrary.Common.DTOs;
using TroyLibrary.Data.Models;
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

        public async Task<BookDetailDTO?> GetBook(int bookId)
        {
            var book = await this._bookRepo.GetBookAsync(bookId);

            if (book == null)
            {
                return null;
            }

            return new BookDetailDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CoverImage = book.CoverImage,
                Rating = book.Reviews?.Average(r => r.Rating),
                IsAvailable = !book.CheckoutDate.HasValue,
                CheckoutDate = book.CheckoutDate,
                PublicationDate = book.PublicationDate,
                ISBN = book.ISBN,
                PageCount = book.PageCount,
                Publisher = book.Publisher,
                CategoryName = book.Category?.Name ?? "N/A",
                Category = (Enums.Category)book.CategoryId,
                Reviews = book.Reviews?.Select(r => new ReviewDTO
                {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    Text = r.Text,
                    Username = r.TroyLibraryUser?.UserName,
                }).ToList()
            };
        }

        public ICollection<BookDTO>? GetFeaturedBooks(int? count = 10)
        { 
            var books = this._bookRepo.GetRandomBooks();

            if (books == null || !books.Any())
            {
                return null;
            }

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

        public ICollection<BookDTO>? SearchBooks(string? title = null)
        {
            var books = this._bookRepo.GetBooks();

            if (books == null || !books.Any())
            {
                return null;
            }

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

        public async Task<DateTime?> CreateBookAsync(BookDataDTO book)
        {
            var b = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CoverImage = book.CoverImage,
                PublicationDate = book.PublicationDate,
                ISBN = book.ISBN,
                PageCount = book.PageCount,
                Publisher = book.Publisher,
                CategoryId = (int)book.Category
            };

             var newBook = await this._bookRepo.CreateBookAsync(b);

            if (newBook != null)
            {
                return DateTime.Now;
            }
            
            return null;
        }

        public async Task<DateTime?> UpdateBookAsync(BookDataDTO book)
        {
            var b = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CoverImage = book.CoverImage,
                PublicationDate = book.PublicationDate,
                ISBN = book.ISBN,
                PageCount = book.PageCount,
                Publisher = book.Publisher,
                CategoryId = (int)book.Category
            };
            var newBook = await this._bookRepo.UpdateBookAsync(b);

            if (newBook != null)
            {
                return DateTime.Now;
            }

            return null;
        }

        public async Task<DateTime?> RemoveBookAsync(int bookId)
        {
            var result = await this._bookRepo.RemoveBookAsync(bookId);

            if (result)
            {
                return DateTime.Now;
            }

            return null;
        }
    }
}
