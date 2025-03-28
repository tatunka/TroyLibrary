namespace TroyLibrary.Common.DTOs
{
    public class BookDetailDTO : BookDTO
    {
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN { get; set; }
        public int PageCount { get; set; }
        public ICollection<ReviewDTO>? Reviews { get; set; }

    }
}
