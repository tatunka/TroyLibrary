using TroyLibrary.Data.Models;

namespace TroyLibrary.Data.Repos.Interfaces
{
    public interface ILookupRepo
    {
        IQueryable<Category> GetCategories();
    }
}
