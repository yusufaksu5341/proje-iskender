using System.Text.Json.Serialization;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Models.Tag;

public class GetAllTagsResult
{
    [JsonPropertyName("tags")] public IEnumerable<Tags> Tags { get; set; }
}