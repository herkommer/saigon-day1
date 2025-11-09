# Step 1 — Static API

Build a minimal API with a hardcoded threshold.

## Goal

Add a `/check/{value}` endpoint that returns whether a value should trigger an alert based on a static threshold (0.5).

## Quick Start

```powershell
dotnet run
```

Test:

```powershell
curl http://localhost:5000/check/0.3   # Should return shouldAlert: false
curl http://localhost:5000/check/0.7   # Should return shouldAlert: true
```

## What to Build

- Add `app.MapGet("/check/{value:float}", ...)` endpoint
- Hardcode threshold: `value > 0.5f`
- Return JSON: `{ value, shouldAlert, method: "hardcoded" }`
