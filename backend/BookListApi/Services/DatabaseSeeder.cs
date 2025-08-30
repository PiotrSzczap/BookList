using BookListApi.Data;
using BookListApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;

namespace BookListApi.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(BookDbContext context)
        {
            try
            {
                // Try to count books - if this fails, the container might not exist yet
                var hasAnyBooks = false;
                try
                {
                    var bookCount = await context.Books.CountAsync();
                    hasAnyBooks = bookCount > 0;
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Container doesn't exist yet, we'll create it when we add the first book
                    hasAnyBooks = false;
                }
                catch
                {
                    // Any other exception, assume we need to seed
                    hasAnyBooks = false;
                }

                if (hasAnyBooks)
                {
                    return; // Database already has data
                }

                var books = new List<Book>
                {
                    new Book
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "The Great Gatsby",
                        Author = "F. Scott Fitzgerald",
                        ISBN = "978-0-7432-7356-5",
                        PublishedDate = new DateTime(1925, 4, 10),
                        Genre = "Fiction",
                        Price = 12.99m,
                        Description = "A classic American novel set in the Jazz Age."
                    },
                    new Book
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "To Kill a Mockingbird",
                        Author = "Harper Lee",
                        ISBN = "978-0-06-112008-4",
                        PublishedDate = new DateTime(1960, 7, 11),
                        Genre = "Fiction",
                        Price = 13.99m,
                        Description = "A gripping tale of racial injustice and childhood innocence."
                    },
                    new Book
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "1984",
                        Author = "George Orwell",
                        ISBN = "978-0-452-28423-4",
                        PublishedDate = new DateTime(1949, 6, 8),
                        Genre = "Dystopian Fiction",
                        Price = 14.99m,
                        Description = "A dystopian social science fiction novel."
                    },
                    new Book
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Pride and Prejudice",
                        Author = "Jane Austen",
                        ISBN = "978-0-14-143951-8",
                        PublishedDate = new DateTime(1813, 1, 28),
                        Genre = "Romance",
                        Price = 11.99m,
                        Description = "A romantic novel of manners set in Georgian England."
                    },
                    new Book
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "The Catcher in the Rye",
                        Author = "J.D. Salinger",
                        ISBN = "978-0-316-76948-0",
                        PublishedDate = new DateTime(1951, 7, 16),
                        Genre = "Fiction",
                        Price = 13.49m,
                        Description = "A controversial novel about teenage rebellion and alienation."
                    }
                };

                context.Books.AddRange(books);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // If seeding fails, don't throw - just continue without seed data
                // In production, you might want to log this error
            }
        }
    }
}
