using Microsoft.EntityFrameworkCore;
using BookListApi.Models;

namespace BookListApi.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .ToContainer("Books")
                .HasPartitionKey(b => b.Id)
                .HasKey(b => b.Id);

            // Configure the Id property to not be auto-generated since we're using GUIDs
            modelBuilder.Entity<Book>()
                .Property(b => b.Id)
                .ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
