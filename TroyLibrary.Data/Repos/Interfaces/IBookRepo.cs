using TroyLibrary.Data.Models;

namespace TroyLibrary.Repo.Interfaces
{
    public interface IBookRepo
    {
        IQueryable<Book> GetBooks();
        IQueryable<Book> GetRandomBooks();
        Task<Book?> GetBookAsync(int bookId);
        Task<Book?> CreateBookAsync(Book book);
        Task<Book?> UpdateBookAsync(Book book);
        Task<bool> RemoveBookAsync(int bookId);
    }
}
