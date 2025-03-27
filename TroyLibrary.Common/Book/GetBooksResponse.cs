using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Common.Book
{
    public class GetBooksResponse
    {
        public ICollection<BookDTO> Books { get; set; }
    }
}
