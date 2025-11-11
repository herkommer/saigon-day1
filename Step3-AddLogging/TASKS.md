# Step 3 Tasks - Add Logging

## Overview

Add structured logging with Serilog to track predictions and enable debugging in production.

## Key Concept

Structured logging captures data as key-value pairs, not just text. This allows querying logs like a database: "Show me all predictions where probability < 0.6 and shouldAlert = true".

## Tasks

### ✅ Task 1: Uncomment Serilog configuration

**File:** `Program.cs` (lines 9-11)

1. Remove `//` from the Serilog configuration
2. Notice the `outputTemplate` - this formats log messages
3. Observe `{Timestamp:HH:mm:ss}` and other placeholders

**Discussion:** Why configure logging before creating the app?

### ✅ Task 2: Uncomment UseSerilog

**File:** `Program.cs` (line 21)

1. Remove `//` from `builder.Host.UseSerilog()`
2. This replaces ASP.NET Core's built-in logging

**Discussion:** What happens if you forget this line?

### ✅ Task 3: Uncomment the models

**File:** `Models.cs` (lines 7-20)

1. Remove `//` from both classes (same as Step 2)

### ✅ Task 4: Uncomment ML.NET setup

**File:** `Program.cs` (lines 31-61)

1. Remove `//` from the entire ML.NET block (same as Step 2)

### ✅ Task 5: Uncomment /predict endpoint with logging

**File:** `Program.cs` (lines 68-87)

1. Remove `//` from the endpoint
2. **Pay special attention** to the `Log.Information` call
3. Notice the template: `"Prediction: Value={Value}, Alert={Alert}, Probability={Probability:F2}"`
4. Notice the parameters match the placeholders

**Discussion:**

- Why use `{Value}` instead of string interpolation like `$"{value}"`?
- What does `:F2` mean in `{Probability:F2}`?

### ✅ Task 6: Run and observe logs

```powershell
dotnet run
```

Test and watch the console:

```powershell
curl http://localhost:5000/predict/0.3
curl http://localhost:5000/predict/0.7
curl http://localhost:5000/predict/0.5
```

**Observe:**

- Timestamps on each log entry
- Log level `[INF]` (Information)
- Structured data in the message
- Probability formatted to 2 decimal places

### ✅ Task 7: Experiment with log queries

If you were using a log aggregator (like Seq, Splunk, or ELK), you could query:

- "All predictions where `Probability < 0.6`"
- "All predictions where `Alert = true`"
- "Average probability for values > 0.7"

**Discussion:**

- Why is this better than `Console.WriteLine($"Value: {value}")`?
- How would you find all low-confidence predictions in text logs?

## Success Criteria ✓

- [ ] Serilog is configured and integrated
- [ ] Running the app shows formatted log output
- [ ] Each `/predict` call logs: Value, Alert, and Probability
- [ ] Probability is formatted to 2 decimal places (e.g., 0.73, not 0.7342...)
- [ ] You understand the difference between structured and text logging

## Key Takeaway

Structured logging is the foundation of **observability**. It lets us:

- Search and filter logs efficiently
- Correlate related events
- Generate metrics from log data
- Debug production issues without redeploying

## Next Step

Move to `Step4-AddOpenTelemetry` to add distributed tracing.
