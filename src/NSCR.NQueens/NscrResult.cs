// SPDX-License-Identifier: Apache-2.0
// Associated publication: https://doi.org/10.3390/electronics13204065

namespace NSCR.NQueens;

public sealed class NscrResult
{
    internal NscrResult(bool solved, int[] rowsByColumn, long operations)
    {
        Solved = solved;
        RowsByColumn = Array.AsReadOnly((int[])rowsByColumn.Clone());
        Operations = operations;
    }

    public bool Solved { get; }

    /// <summary>
    /// Zero-based row position for each zero-based column.
    /// </summary>
    public IReadOnlyList<int> RowsByColumn { get; }

    public long Operations { get; }
}
