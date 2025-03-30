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
    }
}
