// SPDX-License-Identifier: Apache-2.0
// Associated publication: https://doi.org/10.3390/electronics13204065

namespace NSCR.NQueens;

public static class NQueensValidator
{
    public static bool IsValid(IReadOnlyList<int> rowsByColumn)
    {
        ArgumentNullException.ThrowIfNull(rowsByColumn);

        int n = rowsByColumn.Count;

        for (int column = 0; column < n; column++)
        {
            int row = rowsByColumn[column];

            if (row < 0 || row >= n)
            {
                return false;
            }

            for (int otherColumn = column + 1; otherColumn < n; otherColumn++)
            {
                int otherRow = rowsByColumn[otherColumn];

                if (row == otherRow)
                {
                    return false;
                }

                if (Math.Abs(column - otherColumn) == Math.Abs(row - otherRow))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
