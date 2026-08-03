# C# example api client project starter

Example project using [`TurquoiseHealth.Api`](https://www.nuget.org/packages/TurquoiseHealth.Api).

## Setup

```bash
dotnet add package TurquoiseHealth.Api
cp .env.example .env
# fill in TURQUOISE_CLIENT_ID / TURQUOISE_CLIENT_SECRET / TURQUOISE_ORGANIZATION_ID in .env
```

## Run

```bash
dotnet run
```

Runs [Program.cs](Program.cs), which searches for a service package and lists prices for it.
