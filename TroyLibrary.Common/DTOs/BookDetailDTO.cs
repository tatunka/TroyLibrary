using static TroyLibrary.Common.Enums;

namespace TroyLibrary.Common.DTOs
{
    public class BookDetailDTO : BookDTO
    {
        public required string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public required string ISBN { get; set; }
        public int PageCount { get; set; }
        public ICollection<ReviewDTO>? Reviews { get; set; }
        public required Category Category { get; set; }
        public required string CategoryName { get; set; }
        public DateTime? CheckoutDate { get; set; }
    }
}
