using System.Text.Json.Serialization;

namespace Inference.Api;

public class GenerateRequestOptions
{

    [JsonPropertyName("use_cache")]
    public bool UseCache { get; set; } = true;

    [JsonPropertyName("wait_for_model")]
    public bool WaitForModel { get; set; } = false;
}
