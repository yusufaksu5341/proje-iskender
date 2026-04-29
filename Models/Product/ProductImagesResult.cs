using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class ProductImagesResult
{
    [JsonPropertyName("count")] public required int Count { get; set; }
    [JsonPropertyName("images")] public required string[] Images { get; set; }
}