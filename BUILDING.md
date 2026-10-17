# Building

## Requirements

- .NET 8 SDK
- Windows, Linux or macOS

## Restore and build

```bash
dotnet restore NSCR-NQueens.sln
dotnet build NSCR-NQueens.sln --configuration Release --no-restore
```

## Run the smoke tests

```bash
dotnet run --project tests/NSCR.NQueens.SmokeTests --configuration Release
```

## Run the command-line application

```bash
dotnet run --project src/NSCR.NQueens.Cli --configuration Release -- 8
```

For the historical six-queen draft workaround:

```bash
dotnet run --project src/NSCR.NQueens.Cli --configuration Release -- 6 --historical-six-queen
```

## Reproducibility note

This project is a clean reconstruction, not a claim that the resulting binary
is identical to software used during the 2024 experiments.

See `docs/source-audit.md` before using the implementation for replication
work.
