# Step 1 Tasks - Static API

## Overview

Build a minimal API with a hardcoded threshold to determine if a signal value should trigger an alert.

## Tasks

### ✅ Task 1: Uncomment the /check endpoint

**File:** `Program.cs` (lines 8-18)

1. Remove the `//` from lines 8-18 to activate the endpoint
2. Read the code and understand what it does
3. Notice the hardcoded threshold: `value > 0.5f`

### ✅ Task 2: Run and test the API

```powershell
dotnet run
```

In a separate terminal, test the endpoint:

```powershell
# Low value - should NOT alert
curl http://localhost:5000/check/0.3

# High value - should alert
curl http://localhost:5000/check/0.7

# Edge case - right at threshold
curl http://localhost:5000/check/0.5
```

### ✅ Task 3: Discussion Points

Before moving to Step 2, discuss with your group:

1. **What's the problem with this approach?**

   - The threshold is hardcoded at 0.5
   - Cannot adapt to changing patterns
   - What if the optimal threshold is actually 0.42 or 0.63?

2. **What would we need to make this adaptive?**
   - Machine learning to learn from data
   - Feedback loop to improve over time
   - Observability to track predictions

## Success Criteria ✓

- [ ] API runs without errors
- [ ] `/check/0.3` returns `shouldAlert: false`
- [ ] `/check/0.7` returns `shouldAlert: true`
- [ ] You understand why hardcoded thresholds are limiting

## Next Step

When ready, move to `Step2-AddMLNet` to replace the hardcoded threshold with machine learning.
