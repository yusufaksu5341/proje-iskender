using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class CreateProductRequest
{
    /*
     * 
        [FromBody, MaxLength(128)] string name,
       [FromBody] float price,
       [FromBody] DateTime expirationDate,
       [FromBody] uint imageCount
     */
    [MaxLength(128), JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("price")] public required float Price { get; set; }
    [JsonPropertyName("expire")] public required DateTime ExpirationDate { get; set; }
    [JsonPropertyName("image-count")] public required uint ImageCount { get; set; }
}