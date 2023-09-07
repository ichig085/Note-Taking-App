using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteItEasyApp.Models
{
    // NoteModel represents a note created by a user
    public class NoteModel
    {
        [Key]                           
        public string Id { get; set; }           // Primary key

        [Required]
        public string Title { get; set; }        // Property for note's title

        [Required]      
        public string Note { get; set; }         // Property for note

        [ForeignKey("UserModel")]  
        public string UserModelId { get; set; }  // Foreign key relationship with UserModel

        public UserModel UserModel { get; set; } // Navigation property to UserModel
    }
}

