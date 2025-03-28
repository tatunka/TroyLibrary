using TroyLibrary.Data.Models;

namespace TroyLibrary.Repo.Interfaces
{
    public interface IBookRepo
    {
        IQueryable<Book> GetBooks();
        IQueryable<Book> GetRandomBooks();
        Task<Book> GetBook(int bookId);
    }
}
