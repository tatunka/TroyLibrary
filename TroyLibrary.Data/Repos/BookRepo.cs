using Microsoft.EntityFrameworkCore;
using TroyLibrary.Data.Models;
using TroyLibrary.Repo.Interfaces;

namespace TroyLibrary.Data.Repos
{
    public class BookRepo : IBookRepo
    {
        private readonly TroyLibraryContext _context;

        public BookRepo(TroyLibraryContext context)
        {
            _context = context;
        }

        public IQueryable<Book> GetBooks()
        {
            return this._context.Books;
        }

        public IQueryable<Book> GetRandomBooks()
        {
            var random = new Random();
            var skipper = random.Next(0, _context.Books.Count());

            return this._context.Books
                .OrderBy(x => Guid.NewGuid())
                .Skip(skipper);
        }

        public async Task<Book?> GetBookAsync(int bookId)
        {
            return await this._context.Books
                .Include(b => b.Category)
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task<Book?> CreateBookAsync(Book book)
        {
            var b = this._context.Books.Add(book);
            await this._context.SaveChangesAsync();
            return b.Entity;
        }

        public async Task<Book?> UpdateBookAsync(Book book)
        {
            var b = this._context.Books.Update(book);
            await this._context.SaveChangesAsync();
            return b.Entity;
        }

        public async Task<bool> RemoveBookAsync(int bookId)
        {
            var book = await this._context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book == null)
            {
                return false;
            }
            this._context.Books.Remove(book);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckoutBookAsync(int bookId, string userId)
        {
            var book = await this._context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book == null)
            {
                return false;
            }
            book.TroyLibraryUserId = userId;
            book.CheckoutDate = DateTime.Now;
            await this._context.SaveChangesAsync();
            return true;
        }
    }
}
