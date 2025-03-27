using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TroyLibrary.Data.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public string TroyLibraryUserId { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Text { get; set; }

        //Navigation properties
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [ForeignKey("TroyLibraryUserId")]
        public virtual TroyLibraryUser TroyLibraryUser { get; set; }
    }
}
