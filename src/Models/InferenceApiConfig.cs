namespace Inference.Api;

public class InferenceApiConfig
{
   public required string ModelPath { get; init; }

   public required bool CudaEnabled { get; init; } = false;

   public required int GpuLayerCount { get; init; } = 5;

   public required uint ContextSize { get; init; } = 4096;

   public required string Instruction { get; init; }
}
