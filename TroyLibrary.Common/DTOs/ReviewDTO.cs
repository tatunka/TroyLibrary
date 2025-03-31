namespace TroyLibrary.Common.DTOs
{
    public class ReviewDTO
    {
        public required int ReviewId { get; set; }
        public required string Username { get; set; }
        public required int Rating { get; set; }
        public required string Text { get; set; }
    }
}
