# Step 1 - Quick Test Commands

## Run the API

```powershell
dotnet run
```

## Test endpoints

```powershell
# Low value - should not alert
curl http://localhost:5000/check/0.3

# High value - should alert
curl http://localhost:5000/check/0.7

# Edge case - arbitrary!
curl http://localhost:5000/check/0.5
```

## Expected Results

**0.3:**

```json
{ "value": 0.3, "shouldAlert": false, "method": "hardcoded" }
```

**0.7:**

```json
{ "value": 0.7, "shouldAlert": true, "method": "hardcoded" }
```

**0.5:**

```json
{ "value": 0.5, "shouldAlert": true, "method": "hardcoded" }
```

## ✅ Success Criteria

- API runs without errors
- All three test values return expected results
- The threshold of 0.5 is visible in your code
