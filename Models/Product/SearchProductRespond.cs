using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class SearchProductRespond
{
    [JsonPropertyName("count")] public required byte Count { get; set; }
    [JsonPropertyName("prods"), MaxLength(20)] public required SearchRespondBody[] Products { get; set; }
}