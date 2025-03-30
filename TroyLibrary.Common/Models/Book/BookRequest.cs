using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Common.Models.Book
{
    public class BookRequest
    {
        public required BookDataDTO BookData { get; set; }
    }
}
