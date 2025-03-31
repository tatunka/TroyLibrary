namespace TroyLibrary.Common.Models.Review
{
    public class CreateReviewRequest
    {
        public required int BookId { get; set; }
        public required int Rating { get; set; }
        public required string Text { get; set; }
    }
}
