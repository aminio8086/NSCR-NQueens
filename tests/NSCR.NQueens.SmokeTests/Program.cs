// SPDX-License-Identifier: Apache-2.0
// Associated publication: https://doi.org/10.3390/electronics13204065

using NSCR.NQueens;

try
{
    TestUnsolvableSmallBoards();
    TestRepresentativeSolvableBoards();
    TestLargeBoards();
    TestSixQueenAmbiguityIsExplicit();
    TestHistoricalSixQueenWorkaround();
    TestInvalidInput();
    TestOperationLimit();
    TestDeterminism();

    Console.WriteLine("All smoke tests passed.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
    return 1;
}

static void TestUnsolvableSmallBoards()
{
    var solver = new NscrSolver();

    Assert(!solver.Solve(2).Solved, "n=2 must be unsolved.");
    Assert(!solver.Solve(3).Solved, "n=3 must be unsolved.");
}

static void TestRepresentativeSolvableBoards()
{
    var solver = new NscrSolver();

    foreach (int n in new[] { 1, 4, 5, 7, 8, 12, 20, 25 })
    {
        NscrResult result = solver.Solve(n);

        Assert(result.Solved, $"n={n} should be solved.");
        Assert(
            NQueensValidator.IsValid(result.RowsByColumn),
            $"n={n} produced an invalid board.");
    }
}

static void TestLargeBoards()
{
    var solver = new NscrSolver();

    foreach (int n in new[] { 50, 100, 250, 500, 1000 })
    {
        NscrResult result = solver.Solve(n);

        Assert(result.Solved, $"n={n} should be solved.");
        Assert(
            NQueensValidator.IsValid(result.RowsByColumn),
            $"n={n} produced an invalid board.");
    }
}

static void TestSixQueenAmbiguityIsExplicit()
{
    var solver = new NscrSolver();

    try
    {
        solver.Solve(6);
        throw new InvalidOperationException(
            "n=6 should require an explicit reconstruction choice.");
    }
    catch (NotSupportedException)
    {
        // Expected.
    }
}

static void TestHistoricalSixQueenWorkaround()
{
    var solver = new NscrSolver(
        new NscrSolverOptions
        {
            UseHistoricalSixQueenWorkaround = true
        });

    NscrResult result = solver.Solve(6);

    Assert(result.Solved, "Historical n=6 workaround should solve the board.");
    Assert(
        NQueensValidator.IsValid(result.RowsByColumn),
        "Historical n=6 workaround produced an invalid board.");
}

static void TestInvalidInput()
{
    var solver = new NscrSolver();

    try
    {
        solver.Solve(0);
        throw new InvalidOperationException("n=0 should be rejected.");
    }
    catch (ArgumentOutOfRangeException)
    {
        // Expected.
    }
}

static void TestOperationLimit()
{
    var solver = new NscrSolver(
        new NscrSolverOptions
        {
            OperationLimit = 1
        });

    try
    {
        solver.Solve(4);
        throw new InvalidOperationException(
            "A deliberately tiny operation limit should stop the solver.");
    }
    catch (InvalidOperationException)
    {
        // Expected.
    }
}

static void TestDeterminism()
{
    var solver = new NscrSolver();

    NscrResult first = solver.Solve(8);
    NscrResult second = solver.Solve(8);

    Assert(
        first.RowsByColumn.SequenceEqual(second.RowsByColumn),
        "NSCR reconstruction should be deterministic for a fixed n.");
}

static void Assert(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}
