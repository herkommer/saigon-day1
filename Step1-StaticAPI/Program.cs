var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// TODO: Add GET /check/{value:float} endpoint
// Return: { value, shouldAlert (true if value > 0.5), method: "hardcoded" }

app.Run();
