using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjeIskender.Models.Account;

public class UserResult
{
    [JsonPropertyName("user-id")] public required ulong UserId { get; set; }
    [JsonPropertyName("user-name")] public required string UserName { get; set; }
}