using System.ComponentModel.DataAnnotations;

namespace NoteItEasyApp.Models
{
    // UserModel represents a user profile
    public class UserModel
    {
        [Key]  
        public string Id { get; set; }             // Primary key

        public string Name { get; set; }           // User's name

        public string ProfileImage { get; set; }   // URL to user's profile image
    }
}

