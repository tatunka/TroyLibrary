using static TroyLibrary.Common.Enums;

namespace TroyLibrary.Common.DTOs
{
    public class BookDataDTO
    {
        public int? BookId { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Description { get; set; }
        public required string CoverImage { get; set; }
        public required string Publisher { get; set; }
        public required DateTime PublicationDate { get; set; }
        public required Category Category { get; set; }
        public required string ISBN { get; set; }
        public required int PageCount { get; set; }
    }
}
