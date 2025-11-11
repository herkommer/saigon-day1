# Step 4 Tasks - Add OpenTelemetry

## Overview

Add distributed tracing with OpenTelemetry to track requests and understand system behavior. Also start storing observations for future learning.

## Key Concept

**Tracing** shows the flow of a request through your system:

- What operations happened
- How long each took
- What went wrong

Each trace contains **spans** (units of work) with **tags** (metadata).

## Tasks

### ✅ Task 1: Uncomment OpenTelemetry configuration

**File:** `Program.cs` (lines 24-28)

1. Remove `//` from the OpenTelemetry setup
2. Notice `.ConfigureResource(r => r.AddService("AlertPredictor"))` - names our service
3. Notice `.AddAspNetCoreInstrumentation()` - auto-traces HTTP requests
4. Notice `.AddSource("AlertPredictor")` - lets us create custom traces
5. Notice `.AddConsoleExporter()` - outputs traces to console

**Discussion:** What's the difference between logs and traces?

### ✅ Task 2: Uncomment service registration

**File:** `Program.cs` (line 35)

1. Remove `//` from `builder.Services.AddSingleton<ObservationStore>()`
2. This registers the store for dependency injection

**Discussion:** Why singleton? What would happen with scoped or transient?

### ✅ Task 3: Uncomment ActivitySource

**File:** `Program.cs` (line 45)

1. Remove `//` from the `ActivitySource` creation
2. This is our handle for creating custom trace spans

### ✅ Task 4: Uncomment Serilog, models, and ML.NET

**Files:** `Program.cs` and `Models.cs`

1. Uncomment Serilog setup (lines 11-14, 18)
2. Uncomment models in `Models.cs` (lines 7-21)
3. Uncomment ML.NET training code (lines 52-82)

### ✅ Task 5: Uncomment Observation class

**File:** `Models.cs` (lines 28-36)

1. Remove `//` from the `Observation` class
2. Notice `ActualAlert` is nullable - we don't know the truth yet

### ✅ Task 6: Uncomment ObservationStore

**File:** `Models.cs` (lines 44-65)

1. Remove `//` from the `ObservationStore` class
2. Notice it uses `ConcurrentBag<>` for thread safety
3. Notice `GetLabeled()` filters where `ActualAlert.HasValue`

### ✅ Task 7: Uncomment /predict endpoint with tracing

**File:** `Program.cs` (lines 90-124)

1. Remove `//` from the endpoint
2. **Examine the Activity usage:**
   - `using var activity = activitySource.StartActivity(...)` - creates a span
   - `activity?.SetTag(...)` - adds metadata
   - `using` ensures cleanup (no explicit `.Stop()` needed)
3. Notice we create and store an `Observation`
4. Notice the response now includes `observationId`

**Discussion:**

- Why use `using` for the activity?
- What tags would be useful for debugging?
- Why store observations?

### ✅ Task 8: Uncomment /observations endpoint

**File:** `Program.cs` (lines 142-154)

1. Remove `//` from the GET `/observations` endpoint
2. This lets us view what we've predicted

### ✅ Task 9: Run and test with tracing

```powershell
dotnet run
```

Test and observe the console output:

```powershell
# Make some predictions
curl http://localhost:5000/predict/0.3
curl http://localhost:5000/predict/0.7

# Check observations
curl http://localhost:5000/observations
```

**Observe in console:**

- Log messages (from Serilog)
- Trace exports (from OpenTelemetry)
- Activity ID, Parent ID, Duration
- Tags we set

### ✅ Task 10: Discussion - Traces vs Logs

| Aspect        | Logs                | Traces                                          |
| ------------- | ------------------- | ----------------------------------------------- |
| **Purpose**   | What happened       | How it happened                                 |
| **Structure** | Independent events  | Hierarchical spans                              |
| **Best for**  | Debugging, auditing | Performance, flow                               |
| **Example**   | "User logged in"    | "Request took 150ms: 100ms DB, 50ms processing" |

**Question:** If you had a slow request, would you look at logs or traces first?

## Success Criteria ✓

- [ ] OpenTelemetry is configured and running
- [ ] Each `/predict` call creates a trace span
- [ ] Console shows trace exports with Activity ID and duration
- [ ] Trace tags include input value, prediction, and observation ID
- [ ] `/observations` returns stored predictions
- [ ] You understand the difference between logs and traces

## Key Takeaway

We now have:

1. **Logs** - What happened ("Predicted alert for 0.7")
2. **Traces** - How long it took and the flow
3. **Observations** - Stored predictions we can later label

This sets us up for the feedback loop!

## Next Step

Move to `Step5-ObservationStorage` to add labeling capability.
