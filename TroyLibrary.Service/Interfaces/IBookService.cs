using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Service.Interfaces
{
    public interface IBookService
    {
        Task<BookDetailDTO?> GetBook(int bookId);
        ICollection<BookDTO>? SearchBooks(string title);
        ICollection<BookDTO>? GetFeaturedBooks(int? count = 12);
        Task<BookDetailDTO?> CreateBookAsync(BookDataDTO book);
        Task<DateTime?> UpdateBookAsync(BookDataDTO book);
        Task<DateTime?> RemoveBookAsync(int bookId);
        Task<bool> CheckoutBookAsync(int bookId, string? userId);
        Task<bool> ReturnBookAsync(int bookId);
    }
}
