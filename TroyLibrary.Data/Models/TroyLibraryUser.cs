using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TroyLibrary.Data.Models
{
    public class TroyLibraryUser : IdentityUser
    {
        //Navigation properties
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
