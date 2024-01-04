using System.Text.Json.Serialization;

namespace Inference.Api;

public class GenerateRequestParameters
{
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; } = null;

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; } = null;

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = 1.0f;

    [JsonPropertyName("repition_penalty")]
    public float? RepititionPenalty { get; set; } = null;

    [JsonPropertyName("max_new_tokens")]
    public int? MaxNewTokens { get; set; } = null;

    [JsonPropertyName("max_time")]
    public float? MaxTime { get; set; } = null;

    [JsonPropertyName("return_full_text")]
    public bool ReturnFullText { get; set; } = true;

    [JsonPropertyName("num_return_sequences")]
    public int NumReturnSequences { get; set; } = 1;

    [JsonPropertyName("do_sample")]
    public bool DoSample { get; set; } = true;
}
