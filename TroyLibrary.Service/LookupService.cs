using TroyLibrary.Common;
using TroyLibrary.Common.Models.Lookup;
using TroyLibrary.Data.Repos.Interfaces;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.Service
{
    public class LookupService : ILookupService
    {
        private readonly ILookupRepo _lookupRepo;

        public LookupService(ILookupRepo lookupRepo)
        {
            _lookupRepo = lookupRepo;
        }

        public ICollection<LookupItem>? Lookup(string lookupName)
        {
            switch (lookupName)
            {
                case Constants.Lookup.Category:
                    return GetCategories();
                default:
                    return null;
            }
        }

        private ICollection<LookupItem> GetCategories() => 
            _lookupRepo.GetCategories()
                .Select(c => new LookupItem
                {
                    Id = c.CategoryId,
                    Value = c.Name
                })
                .ToList();
    }    
}
