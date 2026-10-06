using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapPost("/tracking/{assetId}", (string assetId, bool fail, ILogger<Program> log) =>
{
    var traceId = Activity.Current?.TraceId.ToString();
    log.LogInformation("Tracking update AssetId={AssetId} TraceId={TraceId} Failed={Failed}",
        assetId, traceId, fail);
    return fail ? Results.StatusCode(503) : Results.Ok(new { assetId, traceId });
});
app.Run();
