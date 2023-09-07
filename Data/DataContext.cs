using Microsoft.EntityFrameworkCore;
using NoteItEasyApp.Models;

namespace NoteItEasyApp.Data
{
    // DataContext class inherits from DbContext, responsible for database interactions
    public class DataContext : DbContext
    {
        // Constructor accepts DbContextOptions, used to configure database connection
        public DataContext(DbContextOptions options) : base(options){}

        // DbSet property to represent the NoteModel entity in the database
        public DbSet<NoteModel> NoteModels { get; set; }

        // DbSet property to represent the UserModel entity in the database
        public DbSet<UserModel> UserModels { get; set; }
        public object NoteModel { get; internal set; }
    }
}

