using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Product;

public class SearchRespondBody
{
   [JsonPropertyName("id")] public required ulong Id { get; set; }
   [JsonPropertyName("name"), MaxLength(128)] public required string Name { get; set; }
   [JsonPropertyName("date")] public required DateTime Date { get; set; }
   [JsonPropertyName("last-price")] public required float Price { get; set; }
}