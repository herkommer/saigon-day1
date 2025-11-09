# Day 1 Morning — Starter Projects

Independent starter projects for each step of the self-learning API workshop. Each folder contains minimal scaffolding with only the packages needed for that step.

## Structure

```
Day1-Starter/
├── Step1-StaticAPI/              # Hardcoded threshold
├── Step2-AddMLNet/               # ML.NET prediction
├── Step3-AddLogging/             # Structured logging with Serilog
├── Step4-AddOpenTelemetry/       # Distributed tracing
├── Step5-ObservationStorage/     # Store predictions for labeling
└── Step6-CompleteWithRetraining/ # Self-learning feedback loop
```

## Usage

Each step is independent — work in any order or skip ahead:

```powershell
# Navigate to any step
cd Step2-AddMLNet

# Restore packages and run
dotnet restore
dotnet run
```

Each folder contains:

- `.csproj` — Pre-configured with required packages for that step
- `Program.cs` — Minimal scaffold with TODO comments
- `Models.cs` — Class placeholders (where applicable)
- `README.md` — Step goals and what to build

## Reference Implementations

Complete working code for each step is available in the parent directory:

- `../Step1-StaticAPI/` through `../Step6-CompleteWithRetraining/`

Use these starter folders to code along during the session.
