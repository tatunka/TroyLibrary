using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;

        public BookService(IBookRepo bookRepo, IConfiguration config)
        {
            _bookRepo = bookRepo;
            _config = config;
        }

        public async Task<BookDetailDTO?> GetBook(int bookId)
        {
            var book = await this._bookRepo.GetBookAsync(bookId);

            if (book == null)
            {
                return null;
            }

            var minutesString = _config["MinutesUntilOverdue"];
            double minutes = 7200;
            if (!string.IsNullOrWhiteSpace(minutesString))
            {
                minutes = double.Parse(_config["MinutesUntilOverdue"]);
            }

            return new BookDetailDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CoverImage = book.CoverImage,
                Rating = book.Reviews?
                    .DefaultIfEmpty()?
                    .Average(r => r?.Rating),
                IsAvailable = !book.CheckoutDate.HasValue,
                CheckoutDate = book.CheckoutDate,
                PublicationDate = book.PublicationDate,
                ISBN = book.ISBN,
                PageCount = book.PageCount,
                Publisher = book.Publisher,
                CategoryName = book.Category.Name,
                Category = (Enums.Category)book.CategoryId,
                IsOverdue = book.CheckoutDate.HasValue && book.CheckoutDate.Value.AddMinutes(minutes) < DateTime.Now,
                DueDate = book.CheckoutDate.HasValue ? book.CheckoutDate.Value.AddMinutes(minutes) : null,
            };
        }

        public ICollection<BookDTO>? GetFeaturedBooks(int? count = 12)
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

            var minutesString = _config["MinutesUntilOverdue"];
            double minutes = 7200;
            if (!string.IsNullOrWhiteSpace(minutesString))
            {
                minutes = double.Parse(_config["MinutesUntilOverdue"]);
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
                    IsAvailable = !b.CheckoutDate.HasValue,
                    IsOverdue = b.CheckoutDate.HasValue && b.CheckoutDate.Value.AddMinutes(minutes) < DateTime.Now,
                    DueDate = b.CheckoutDate.HasValue ? b.CheckoutDate.Value.AddMinutes(minutes) : null,
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

            if (!string.IsNullOrWhiteSpace(title) && title != "*")
            {
                books = books.Where(b => b.Title.ToLower().Contains(title.ToLower()));
            }

            var minutesString = _config["MinutesUntilOverdue"];
            double minutes = 7200;
            if (!string.IsNullOrWhiteSpace(minutesString))
            {
                minutes = double.Parse(_config["MinutesUntilOverdue"]);
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
                    IsAvailable = !b.CheckoutDate.HasValue,
                    IsOverdue = b.CheckoutDate.HasValue && b.CheckoutDate.Value.AddMinutes(minutes) < DateTime.Now,
                    DueDate = b.CheckoutDate.HasValue ? b.CheckoutDate.Value.AddMinutes(minutes) : null,
                })
                .ToList();
        }

        public async Task<BookDetailDTO?> CreateBookAsync(BookDataDTO book)
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
                CategoryId = (int)book.Category,
            };

            var newBook = await this._bookRepo.CreateBookAsync(b);

            if (newBook != null)
            {
                return new BookDetailDTO
                {
                    BookId = newBook.BookId,
                    Title = newBook.Title,
                    Author = newBook.Author,
                    Description = newBook.Description,
                    CoverImage = newBook.CoverImage,
                    Publisher = newBook.Publisher,
                    PublicationDate = newBook.PublicationDate,
                    ISBN = newBook.ISBN,
                    PageCount = newBook.PageCount,
                    Category = (Enums.Category)newBook.CategoryId,
                    CategoryName = newBook.Category?.Name ?? Enum.GetName(typeof (Enums.Category), newBook.CategoryId) ?? "Unknown",
                };
            }
            
            return null;
        }

        public async Task<DateTime?> UpdateBookAsync(BookDataDTO book)
        {
            if (!book.BookId.HasValue)
            {
                return null;
            }

            var b = await this._bookRepo.GetBookAsync(book.BookId.Value);
            if (b == null)
            {
                return null;
            }

            b.Title = book.Title;
            b.Author = book.Author;
            b.Description = book.Description;
            b.CoverImage = book.CoverImage;
            b.PublicationDate = book.PublicationDate;
            b.ISBN = book.ISBN;
            b.PageCount = book.PageCount;
            b.Publisher = book.Publisher;
            b.CategoryId = (int)book.Category;

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

        public async Task<bool> CheckoutBookAsync(int bookId, string? userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var book = await this._bookRepo.GetBookAsync(bookId);

            if (book == null || book.CheckoutDate.HasValue)
            {
                return false;
            }
            book.CheckoutDate = DateTime.Now;
            book.TroyLibraryUserId = userId;
            var result = await this._bookRepo.UpdateBookAsync(book);
            return result != null;
        }

        public async Task<bool> ReturnBookAsync(int bookId)
        {
            var book = await this._bookRepo.GetBookAsync(bookId);
            if (book == null || !book.CheckoutDate.HasValue)
            {
                return false;
            }
            book.CheckoutDate = null;
            book.TroyLibraryUserId = null;
            var result = await this._bookRepo.UpdateBookAsync(book);
            return result != null;
        }
    }
}
