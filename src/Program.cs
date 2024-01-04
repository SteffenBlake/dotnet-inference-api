using System.Text;
using System.Text.Json;
using Inference.Api;
using LLama;
using LLama.Common;
using LLama.Native;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.Get<InferenceApiConfig>() ?? throw new JsonException();

NativeLibraryConfig.Instance.WithCuda(config.CudaEnabled);

var modelParams = new ModelParams(config.ModelPath)
{
    ContextSize = config.ContextSize
};

if (!config.CudaEnabled)
{
    modelParams.GpuLayerCount = 0;
} 
else 
{
   modelParams.GpuLayerCount = config.GpuLayerCount; 
}


using var llm = LLamaWeights.LoadFromFile(modelParams);
using var llmCtx = llm.CreateContext(modelParams);
var executor = new InstructExecutor(llmCtx);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    }
);

app.MapPost("/generate", async ([FromBody]GenerateRequest request) =>
{
    var infParams = new InferenceParams()
    {
        Temperature = request.Parameters.Temperature,
    };

    if (request.Parameters.TopK.HasValue)
    {
        infParams.TopK = request.Parameters.TopK.Value;
    }
    if (request.Parameters.TopP.HasValue)
    {
        infParams.TopP = request.Parameters.TopP.Value;
    }
    if (request.Parameters.RepititionPenalty.HasValue)
    {
        infParams.RepeatPenalty = request.Parameters.RepititionPenalty.Value;
    }
    if (request.Parameters.MaxNewTokens.HasValue)
    {
        infParams.MaxTokens = request.Parameters.MaxNewTokens.Value;
    }

    var tokenSrc = new CancellationTokenSource();
    var genTask = GenerateResponse(request.Inputs, infParams, tokenSrc.Token);
    List<Task> tasks = [genTask];
    if (request.Parameters.MaxTime.HasValue)
    {
        var maxTimeMs = request.Parameters.MaxTime.Value * 1000;
        tasks.Add(Task.Delay((int)maxTimeMs));
    }

    var finished = await Task.WhenAny(tasks);
    if (finished != genTask)
    {
        tokenSrc.Cancel();
    }

    return await genTask;
})
.WithName("Generate")
.WithOpenApi();

app.Run();


async Task<string> GenerateResponse(
        string input,
        InferenceParams infParams, 
        CancellationToken cancellationToken
    )
{
    var compiledInput = config.Instruction + "\n" + input;

    var response = new StringBuilder();

    await foreach(var tkn in executor.InferAsync(compiledInput, infParams))
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return response.ToString();
        }

        response.Append(tkn);
    }

    return response.ToString();
}

