using TroyLibrary.Data.Models;

namespace TroyLibrary.Repo.Interfaces
{
    public interface IBookRepo
    {
        public IQueryable<Book> GetBooks();
        IQueryable<Book> GetRandomBooks();
    }
}
