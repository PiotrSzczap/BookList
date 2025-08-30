using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BookListApi.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadContentAsync(string bookId, Stream contentStream, string fileName);
        Task<Stream> DownloadContentAsync(string contentLink);
        Task<bool> DeleteContentAsync(string contentLink);
        Task<string> GetContentUrlAsync(string contentLink);
    }

    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "book-content";

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadContentAsync(string bookId, Stream contentStream, string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                
                // Ensure container exists
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
                
                // Generate unique blob name
                var fileExtension = Path.GetExtension(fileName);
                var blobName = $"{bookId}/content{fileExtension}";
                
                var blobClient = containerClient.GetBlobClient(blobName);
                
                // Set content type based on file extension
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = GetContentType(fileExtension)
                };
                
                // Upload the content
                await blobClient.UploadAsync(contentStream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders,
                    Conditions = null // Overwrite if exists
                });
                
                return blobName;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to upload content: {ex.Message}", ex);
            }
        }

        public async Task<Stream> DownloadContentAsync(string contentLink)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(contentLink);
                
                var response = await blobClient.DownloadStreamingAsync();
                return response.Value.Content;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to download content: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteContentAsync(string contentLink)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(contentLink);
                
                var response = await blobClient.DeleteIfExistsAsync();
                return response.Value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete content: {ex.Message}", ex);
            }
        }

        public async Task<string> GetContentUrlAsync(string contentLink)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(contentLink);
                
                // Check if blob exists
                var exists = await blobClient.ExistsAsync();
                if (!exists.Value)
                {
                    throw new FileNotFoundException($"Content not found: {contentLink}");
                }
                
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get content URL: {ex.Message}", ex);
            }
        }

        private static string GetContentType(string fileExtension)
        {
            return fileExtension.ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".html" => "text/html",
                ".epub" => "application/epub+zip",
                ".mobi" => "application/x-mobipocket-ebook",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".doc" => "application/msword",
                _ => "application/octet-stream"
            };
        }
    }
}
