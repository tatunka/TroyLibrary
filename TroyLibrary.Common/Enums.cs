using System.ComponentModel.DataAnnotations;

namespace TroyLibrary.Common
{
    public class Enums
    {
        public enum Role
        {
            [Display(Name = "Librarian")]
            Librarian = 1,
            [Display(Name = "Customer")]
            Customer
        }

        public enum Category
        {
            [Display(Name = "Non-Fiction")]
            NonFiction = 1,
            [Display(Name = "Science Fiction")]
            ScienceFiction,
            [Display(Name = "Fantasy")]
            Fantasy,
            [Display(Name = "Mystery")]
            Mystery,
            [Display(Name = "Romance")]
            Romance,
            [Display(Name = "Horror")]
            Horror,
            [Display(Name = "Thriller")]
            Thriller,
            [Display(Name = "Biography")]
            Biography,
            [Display(Name = "History")]
            History,
            [Display(Name = "Self-Help")]
            SelfHelp
        }
    }
}
