// SPDX-License-Identifier: Apache-2.0
// Associated publication: https://doi.org/10.3390/electronics13204065

using NSCR.NQueens;

if (args.Length == 0 || !int.TryParse(args[0], out int queenCount))
{
    Console.Error.WriteLine(
        "Usage: dotnet run --project src/NSCR.NQueens.Cli -- <n> " +
        "[--historical-six-queen]");
    return 1;
}

bool useHistoricalSixQueen =
    args.Any(
        argument =>
            string.Equals(
                argument,
                "--historical-six-queen",
                StringComparison.OrdinalIgnoreCase));

var solver = new NscrSolver(
    new NscrSolverOptions
    {
        UseHistoricalSixQueenWorkaround = useHistoricalSixQueen
    });

try
{
    NscrResult result = solver.Solve(queenCount);

    Console.WriteLine($"Queens: {queenCount}");
    Console.WriteLine($"Solved: {result.Solved}");
    Console.WriteLine($"Operations: {result.Operations}");

    if (result.Solved)
    {
        Console.WriteLine(
            "Rows by column (1-based): " +
            string.Join(", ", result.RowsByColumn.Select(row => row + 1)));
    }

    return result.Solved ? 0 : 2;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);
    return 3;
}
