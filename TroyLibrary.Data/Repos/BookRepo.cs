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
            return this._context.Books.Where(b => b.InStock);
        }

        public IQueryable<Book> GetRandomBooks()
        {
            var random = new Random();
            var skipper = random.Next(0, _context.Books.Count());

            return this._context.Books
                .OrderBy(x => Guid.NewGuid())
                .Skip(skipper);
        }
    }
}
