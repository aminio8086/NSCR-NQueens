# NSCR for the N-Queens Problem

Reference C# reconstruction of the **Non-Sequential Conflict Resolution (NSCR)**
algorithm associated with:

> Omid Moghimi and Amin Amini,  
> **"A Novel Approach for Solving the N-Queen Problem Using a Non-Sequential Conflict Resolution Algorithm"**,  
> *Electronics*, 13(20), 4065, 2024.  
> https://doi.org/10.3390/electronics13204065

NSCR was introduced as a dynamic conflict-resolution approach to the N-Queens
problem. The associated paper reports linear auxiliary-space behaviour and a
worst-case time complexity of O(n³), and evaluates the method for problem sizes
up to 1000 queens.

## Important reconstruction note

The original publication source code was not available as a clean repository.
This release was therefore reconstructed from two sources:

1. the **published paper**, which is treated as the primary scientific source;
2. a recovered historical C# experimental file supplied by the authors.

The historical file is clearly related to NSCR but does **not** exactly match
the final paper in every detail. For that reason this repository does not claim
that its implementation is the byte-for-byte source used to generate the
published experimental results.

See [docs/source-audit.md](docs/source-audit.md) for the discrepancies and the
reconstruction policy.

## Repository contents

```text
src/
├── NSCR.NQueens/
│   ├── NscrSolver.cs
│   ├── NscrSolverOptions.cs
│   ├── NscrResult.cs
│   └── NQueensValidator.cs
└── NSCR.NQueens.Cli/
    └── Program.cs

tests/
└── NSCR.NQueens.SmokeTests/
    └── Program.cs

docs/
├── algorithm.md
├── source-audit.md
├── validation.md
└── provenance.md
```

## Quick start

Requirements:

- .NET 8 SDK

Build:

```bash
dotnet build NSCR-NQueens.sln --configuration Release
```

Run the solver:

```bash
dotnet run --project src/NSCR.NQueens.Cli -- 8
```

Example output contains the zero-conflict board as a list of **1-based row
positions by column**.

## API

```csharp
using NSCR.NQueens;

var solver = new NscrSolver();
NscrResult result = solver.Solve(8);

if (result.Solved)
{
    Console.WriteLine(string.Join(", ", result.RowsByColumn));
}
```

The library stores one queen position per column and keeps additional
O(n)-sized state arrays, reflecting the representation used by the recovered
NSCR implementation.

## Six-queen case

The six-queen case contains a genuine source discrepancy.

The historical C# draft special-cases six queens with:

```text
Qy[0] = 2
```

which represents the **third row** when interpreted as zero-based data.

The paper, however, describes placing the first queen in the **fourth row** for
the six-queen workaround.

The repository therefore refuses to make a silent choice for n=6. By default:

```csharp
new NscrSolver().Solve(6);
```

throws `NotSupportedException` with a pointer to the source audit.

The historical draft behaviour can be requested explicitly:

```csharp
var solver = new NscrSolver(
    new NscrSolverOptions
    {
        UseHistoricalSixQueenWorkaround = true
    });

NscrResult result = solver.Solve(6);
```

This makes the reconstruction decision visible rather than rewriting the
research record.

## Testing

The smoke-test project validates:

- known unsolvable cases n=2 and n=3;
- valid solutions for representative solvable sizes;
- the explicit six-queen ambiguity;
- the recovered historical six-queen workaround;
- deterministic output for a fixed problem size;
- final N-Queens board validity.

GitHub Actions builds and runs the smoke tests on both Windows and Linux.

## Benchmarking

This repository deliberately does **not** reproduce the paper's runtime or
memory tables from the supplied draft file.

The recovered file contains an experimental benchmark harness, but its loop,
sample counts and labels do not fully match the final publication. Recreating
published benchmark numbers from that harness would therefore overstate the
evidence.

A future benchmark suite should be added only with a clearly documented
methodology and should distinguish new measurements from the 2024 published
results.

## Attribution

The paper lists **Omid Moghimi** and **Amin Amini** as authors.

Its author-contribution statement attributes conceptualisation and algorithm
development to Omid Moghimi, and validation to Omid Moghimi and Amin Amini,
with formal analysis and methodology attributed to Amin Amini.

This repository preserves that distinction rather than presenting the NSCR
algorithm as the sole work of either author.

## Citation

If you use this implementation in scholarly work, please cite the associated
paper:

> Moghimi, O.; Amini, A. A Novel Approach for Solving the N-Queen Problem
> Using a Non-Sequential Conflict Resolution Algorithm. *Electronics* 2024,
> 13(20), 4065. https://doi.org/10.3390/electronics13204065

A machine-readable [CITATION.cff](CITATION.cff) is included.

## Licence

The software in this repository is distributed under the Apache License,
Version 2.0. See [LICENSE](LICENSE) and [NOTICE](NOTICE).

The associated paper is a separate publication distributed by its publisher
under its stated open-access terms.
