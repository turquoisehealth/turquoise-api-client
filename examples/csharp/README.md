# C# example api client project starter

Example project using [`TurquoiseHealth.Api`](https://www.nuget.org/packages/TurquoiseHealth.Api).

## Setup

Requires the .NET 10 SDK. From this directory:

```bash
cp .env.example .env
# fill in TURQUOISE_CLIENT_ID / TURQUOISE_CLIENT_SECRET / TURQUOISE_ORGANIZATION_ID in .env
```

## Run

```bash
dotnet run
```

Runs [Program.cs](Program.cs), which searches for a service package and lists prices for it.

### Use the local SDK

By default, the example uses the published NuGet package. To build and run against
the SDK source in this repository instead:

```bash
dotnet run -p:UseLocalSdk=true
```

This uses a project reference to `../../csharp/src/TurquoiseHealth.Api` and builds
the local SDK automatically. To check compilation without making API calls:

```bash
dotnet build -p:UseLocalSdk=true
```
