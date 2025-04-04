﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TroyLibrary.Data.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        public int CategoryId { get; set; }
        public string? TroyLibraryUserId { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        [Required]
        public required string Author { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string CoverImage { get; set; }
        [Required]
        public required string Publisher { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [StringLength(13)]
        [Required]
        public required string ISBN { get; set; }
        [Required]
        public int PageCount { get; set; }
        public DateTime? CheckoutDate { get; set; }

        //Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("TroyLibraryUserId")]
        public virtual TroyLibraryUser TroyLibraryUser { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
