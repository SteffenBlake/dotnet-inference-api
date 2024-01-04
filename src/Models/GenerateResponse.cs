using System.Text.Json.Serialization;

namespace Inference.Api;

public class GenerateResponse
{
    [JsonPropertyName("generated_text")]
    public required string GeneratedText { get; init; }
}
