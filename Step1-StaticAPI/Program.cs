var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ============================================================================
// TASK 1: Uncomment the endpoint below (lines 8-18)
// ============================================================================
// This creates a GET endpoint that accepts a float value and returns whether
// it should trigger an alert based on a hardcoded threshold of 0.5
// ============================================================================

// app.MapGet("/check/{value:float}", (float value) =>
// {
//     // TASK 2: Examine this threshold - this is our hardcoded decision logic
//     var shouldAlert = value > 0.5f;
//
//     return new
//     {
//         value,
//         shouldAlert,
//         method = "hardcoded"
//     };
// });

app.Run();
