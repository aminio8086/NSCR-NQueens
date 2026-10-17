// SPDX-License-Identifier: Apache-2.0
// Associated publication: https://doi.org/10.3390/electronics13204065

namespace NSCR.NQueens;

/// <summary>
/// Clean reconstruction of the NSCR state machine from the associated paper
/// and a recovered historical C# draft.
///
/// This class is not claimed to be the exact source used to generate the
/// published benchmark tables. See docs/source-audit.md.
/// </summary>
public sealed class NscrSolver
{
    private const int NoConflict = -1;
    private const int MultipleConflicts = -2;

    private readonly NscrSolverOptions _options;

    public NscrSolver()
        : this(new NscrSolverOptions())
    {
    }

    public NscrSolver(NscrSolverOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        if (_options.OperationLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "OperationLimit must be greater than zero.");
        }
    }

    public NscrResult Solve(int queenCount)
    {
        if (queenCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(queenCount),
                "The number of queens must be greater than zero.");
        }

        if (queenCount == 1)
        {
            return new NscrResult(true, new[] { 0 }, 0);
        }

        if (queenCount is 2 or 3)
        {
            return new NscrResult(false, new int[queenCount], 0);
        }

        if (queenCount == 6 && !_options.UseHistoricalSixQueenWorkaround)
        {
            throw new NotSupportedException(
                "The recovered C# draft and the publication disagree on the " +
                "six-queen initial-row workaround. See docs/source-audit.md. " +
                "Set UseHistoricalSixQueenWorkaround=true to reproduce the " +
                "draft behaviour explicitly.");
        }

        var rowsByColumn = new int[queenCount];
        var previousConflictColumn = new int[queenCount];
        var isPlaced = new bool[queenCount];

        // The recovered draft uses zero-initialised conflict markers.
        // Retaining that initial state avoids changing the historical
        // state-machine behaviour without evidence.
        Array.Fill(previousConflictColumn, 0);

        if (queenCount == 6)
        {
            // Historical draft: Qy[0] = 2, i.e. third row using zero-based data.
            rowsByColumn[0] = 2;
        }

        int currentColumn = 0;
        int placedCount = 0;
        int conflict = NoConflict;
        long operations = 0;

        while (placedCount < queenCount || conflict != NoConflict)
        {
            EnsureOperationBudget(++operations);

            conflict = FindConflict(
                currentColumn,
                rowsByColumn,
                isPlaced);

            if (conflict >= 0)
            {
                previousConflictColumn[currentColumn] = conflict;
                previousConflictColumn[conflict] = currentColumn;

                if (isPlaced[currentColumn])
                {
                    currentColumn = conflict;
                }

                MoveDown(
                    currentColumn,
                    rowsByColumn,
                    previousConflictColumn,
                    isPlaced,
                    ref operations);
            }
            else if (conflict == NoConflict)
            {
                isPlaced[currentColumn] = true;
                placedCount++;
                currentColumn = placedCount;
            }
            else
            {
                MoveDown(
                    currentColumn,
                    rowsByColumn,
                    previousConflictColumn,
                    isPlaced,
                    ref operations);
            }
        }

        bool solved = NQueensValidator.IsValid(rowsByColumn);

        return new NscrResult(solved, rowsByColumn, operations);
    }

    private static int FindConflict(
        int queenColumn,
        int[] rowsByColumn,
        bool[] isPlaced)
    {
        int threatCount = 0;
        int conflictingColumn = NoConflict;

        for (int column = 0; column < rowsByColumn.Length; column++)
        {
            if (column == queenColumn || !isPlaced[column])
            {
                continue;
            }

            bool sameRow =
                rowsByColumn[column] == rowsByColumn[queenColumn];

            bool sameDescendingDiagonal =
                column - rowsByColumn[column] ==
                queenColumn - rowsByColumn[queenColumn];

            bool sameAscendingDiagonal =
                column + rowsByColumn[column] ==
                queenColumn + rowsByColumn[queenColumn];

            if (!sameRow && !sameDescendingDiagonal && !sameAscendingDiagonal)
            {
                continue;
            }

            conflictingColumn = column;
            threatCount++;

            // The recovered draft continues until more than two threats even
            // though any count above one maps to MultipleConflicts.
            if (threatCount > 2)
            {
                break;
            }
        }

        return threatCount > 1
            ? MultipleConflicts
            : conflictingColumn;
    }

    private void MoveDown(
        int queenColumn,
        int[] rowsByColumn,
        int[] previousConflictColumn,
        bool[] isPlaced,
        ref long operations)
    {
        int homeRow = rowsByColumn[queenColumn];

        for (int attempt = 0; attempt < rowsByColumn.Length; attempt++)
        {
            EnsureOperationBudget(++operations);

            rowsByColumn[queenColumn] =
                (rowsByColumn[queenColumn] + 1) % rowsByColumn.Length;

            if (rowsByColumn[queenColumn] == 0)
            {
                isPlaced[queenColumn] = true;
            }

            int conflict =
                FindConflict(queenColumn, rowsByColumn, isPlaced);

            if (conflict >= 0 &&
                conflict != previousConflictColumn[queenColumn])
            {
                previousConflictColumn[queenColumn] = conflict;

                // The historical draft writes the movement-loop index into
                // the reciprocal conflict slot. That mixes row-attempt and
                // queen-index semantics. This reconstruction stores the
                // reciprocal queen column instead; see docs/source-audit.md.
                previousConflictColumn[conflict] = queenColumn;
                return;
            }

            if (conflict == NoConflict)
            {
                isPlaced[queenColumn] = true;
                return;
            }

            if (rowsByColumn[queenColumn] == homeRow)
            {
                isPlaced[queenColumn] = true;
                return;
            }
        }
    }

    private void EnsureOperationBudget(long operations)
    {
        if (operations > _options.OperationLimit)
        {
            throw new InvalidOperationException(
                "NSCR operation limit exceeded before a solution was found. " +
                "Increase OperationLimit only after reviewing the current " +
                "state-machine behaviour.");
        }
    }
}
