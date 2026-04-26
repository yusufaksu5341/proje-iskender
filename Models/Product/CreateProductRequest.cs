using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class CreateProductRequest
{
    [MaxLength(128), JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("price")] public required float Price { get; set; }
}