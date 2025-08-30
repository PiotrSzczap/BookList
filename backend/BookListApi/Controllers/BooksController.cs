using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookListApi.Models;
using BookListApi.Data;
using BookListApi.Services;

namespace BookListApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IBlobStorageService _blobStorageService;

        public BooksController(BookDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            try
            {
                // Ensure a new ID is generated
                book.Id = Guid.NewGuid().ToString();
                
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, Book book)
        {
            try
            {
                if (id != book.Id)
                {
                    return BadRequest("ID mismatch");
                }

                var existingBook = await _context.Books.FindAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.PublishedDate = book.PublishedDate;
                existingBook.Genre = book.Genre;
                existingBook.Price = book.Price;
                existingBook.Description = book.Description;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/content")]
        public async Task<ActionResult> UploadBookContent(string id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                // Check if book exists
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Book not found");
                }

                // Upload content to blob storage
                using var stream = file.OpenReadStream();
                var contentUrl = await _blobStorageService.UploadContentAsync(
                    $"{id}_{file.FileName}", 
                    stream, 
                    file.ContentType
                );

                // Delete old content if exists
                if (!string.IsNullOrEmpty(book.ContentLink))
                {
                    // Extract blob name from URL
                    var oldBlobName = Path.GetFileName(new Uri(book.ContentLink).AbsolutePath);
                    await _blobStorageService.DeleteContentAsync(oldBlobName);
                }

                // Update book with new content link
                book.ContentLink = contentUrl;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Content uploaded successfully", contentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/content")]
        public async Task<ActionResult> DownloadBookContent(string id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Book not found");
                }

                if (string.IsNullOrEmpty(book.ContentLink))
                {
                    return NotFound("No content available for this book");
                }

                // Extract blob name from URL
                var blobName = Path.GetFileName(new Uri(book.ContentLink).AbsolutePath);
                var contentStream = await _blobStorageService.DownloadContentAsync(blobName);

                if (contentStream == null)
                {
                    return NotFound("Content not found in storage");
                }

                // Determine content type based on file extension
                var extension = Path.GetExtension(blobName).ToLowerInvariant();
                var contentType = extension switch
                {
                    ".pdf" => "application/pdf",
                    ".epub" => "application/epub+zip",
                    ".txt" => "text/plain",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".doc" => "application/msword",
                    _ => "application/octet-stream"
                };

                return File(contentStream, contentType, blobName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}/content")]
        public async Task<ActionResult> DeleteBookContent(string id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Book not found");
                }

                if (string.IsNullOrEmpty(book.ContentLink))
                {
                    return NotFound("No content to delete");
                }

                // Extract blob name from URL
                var blobName = Path.GetFileName(new Uri(book.ContentLink).AbsolutePath);
                await _blobStorageService.DeleteContentAsync(blobName);

                // Clear content link from book
                book.ContentLink = null;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Content deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/content/url")]
        public async Task<ActionResult> GetBookContentUrl(string id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Book not found");
                }

                if (string.IsNullOrEmpty(book.ContentLink))
                {
                    return NotFound("No content available for this book");
                }

                // Extract blob name from URL
                var blobName = Path.GetFileName(new Uri(book.ContentLink).AbsolutePath);
                var downloadUrl = await _blobStorageService.GetContentUrlAsync(blobName);

                return Ok(new { downloadUrl, expiresIn = "1 hour" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
