namespace TroyLibrary.Common.DTOs
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public double? Rating { get; set; }
        public bool IsAvailable { get; set; }
    }
}
