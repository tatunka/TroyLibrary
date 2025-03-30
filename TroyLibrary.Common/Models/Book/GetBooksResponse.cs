using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Common.Models.Book
{
    public class GetBooksResponse
    {
        public ICollection<BookDTO> Books { get; set; }
    }
}
