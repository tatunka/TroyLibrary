using TroyLibrary.Common.Models.Lookup;

namespace TroyLibrary.Service.Interfaces
{
    public interface ILookupService
    {
        ICollection<LookupItem>? Lookup(string lookupName);
    }
}
