using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Service.Interfaces
{
    public interface IBookService
    {
        Task<BookDetailDTO?> GetBook(int bookId);
        ICollection<BookDTO>? SearchBooks(string title);
        ICollection<BookDTO>? GetFeaturedBooks(int? count = 10);
        Task<DateTime?> CreateBookAsync(BookDataDTO book);
        Task<DateTime?> UpdateBookAsync(BookDataDTO book);
        Task<DateTime?> RemoveBookAsync(int bookId);
    }
}
