using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookListApi.Models
{
    public class Book
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [JsonPropertyName("title")]
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [JsonPropertyName("author")]
        [Required]
        public string Author { get; set; } = string.Empty;
        
        [JsonPropertyName("isbn")]
        [Required]
        public string ISBN { get; set; } = string.Empty;
        
        [JsonPropertyName("publishedDate")]
        public DateTime PublishedDate { get; set; }
        
        [JsonPropertyName("genre")]
        [Required]
        public string Genre { get; set; } = string.Empty;
        
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("contentLink")]
        public string? ContentLink { get; set; }
    }
}
