using System.Text.Json.Serialization;

namespace Inference.Api;

public class GenerateRequest 
{
    [JsonPropertyName("inputs")]
    public required string Inputs { get; init; }

    [JsonPropertyName("parameters")]
    public GenerateRequestParameters Parameters { get; init; } = new ();

    [JsonPropertyName("options")]
    public GenerateRequestOptions Options { get; init; } = new ();
}

