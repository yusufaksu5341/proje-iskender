using System.Text.Json.Serialization;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Models.Product;

public class AllBidsResult
{
    [JsonPropertyName("prices")] public required IEnumerable<ProductPrice> Prices { get; set; }
}