using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Service.Interfaces
{
    public interface IBookService
    {
        ICollection<BookDTO> SearchBooks(string title);
        ICollection<BookDTO> GetFeaturedBooks(int? count = 10);
    }
}
