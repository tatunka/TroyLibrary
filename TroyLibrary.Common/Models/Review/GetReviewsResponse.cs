using TroyLibrary.Common.DTOs;

namespace TroyLibrary.Common.Models.Review
{
    public class GetReviewsResponse
    {
        public required ICollection<ReviewDTO> Reviews { get; set; }
    }
}
