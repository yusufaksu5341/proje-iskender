using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class CreateProductRequest
{
    [MaxLength(128), JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("price")] public required float Price { get; set; }
    [JsonPropertyName("single-price")] public required bool SinglePrice { get; set; }
    [JsonPropertyName("expire")] public required DateTime? ExpirationDate { get; set; } = null;
    [JsonPropertyName("details")] public object? Details { get; set; } = null;
}