var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// TODO: Add GET /check/{value:float} endpoint
// Return: { value, shouldAlert (true if value > 0.5), method: "hardcoded" }

app.MapGet("/", () => "I am alive");
app.MapGet("/check/{value:float}", (float value) =>
{
    var shouldAlert = value > 0.5f;

    return new
    {
        value,
        shouldAlert,
        method = "hardcoded"
    };
});

app.Run();
