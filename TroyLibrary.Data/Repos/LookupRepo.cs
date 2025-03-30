using TroyLibrary.Data.Models;
using TroyLibrary.Data.Repos.Interfaces;

namespace TroyLibrary.Data.Repos
{
    public class LookupRepo : ILookupRepo
    {
        private readonly TroyLibraryContext _context;

        public LookupRepo(TroyLibraryContext context) 
        {
            this._context = context;
        }

        public IQueryable<Category> GetCategories()
        {
            return _context.Categories;
        }
    }
}
