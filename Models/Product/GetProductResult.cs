using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class GetProductResult
{
    [JsonPropertyName("creation-date")] public required DateTime CreationDate { get; set; }
    [JsonPropertyName("expiration-date")] public required DateTime ExpirationDate { get; set; }
    [JsonPropertyName("product-name")] public required string Name { get; set; }
    [JsonPropertyName("starting-price")] public required float StartingPrice { get; set; }
    [JsonPropertyName("current-price")] public required float CurrentPrice { get; set; }
    [JsonPropertyName("single-price")] public bool SinglePrice { get; set; } = false;

    [JsonPropertyName("details")] public JsonElement? Details { get; set; } = null;
    [JsonPropertyName("main-image")] public string? MainImage { get; set; } = null;
}