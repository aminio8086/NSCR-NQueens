// SPDX-License-Identifier: Apache-2.0
// Associated publication: https://doi.org/10.3390/electronics13204065

namespace NSCR.NQueens;

public sealed class NscrSolverOptions
{
    /// <summary>
    /// Enables the six-queen initial-row workaround found in the recovered
    /// historical C# draft (zero-based row 2 / third row).
    ///
    /// The publication instead states fourth row. Because those two sources
    /// disagree, six queens are rejected by default rather than silently
    /// choosing one interpretation.
    /// </summary>
    public bool UseHistoricalSixQueenWorkaround { get; init; }

    /// <summary>
    /// Maximum number of state-changing operations before the solver stops.
    /// This guard was not present in the historical draft.
    /// </summary>
    public long OperationLimit { get; init; } = 100_000_000;
}
