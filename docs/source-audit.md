# Source audit

## Purpose

The source supplied for reconstruction was a single historical C# experimental
file. It is strongly related to the published NSCR work, but several details
show that it should not be treated blindly as the exact final publication
source.

The clean implementation therefore follows this evidence order:

1. published paper for scientific naming, scope and claims;
2. recovered C# draft for concrete state-machine behaviour;
3. explicit documented reconstruction decisions where the two do not agree.

## Strong matches

The recovered draft contains an `OmidAlgorithm(int n)` method whose internal
state maps directly onto the NSCR approach:

- one row value per queen/column;
- a conflict-pointer collection;
- placement flags;
- a threat checker for row and two diagonals;
- a movement routine that relocates queens;
- conflict-directed reassignment rather than only sequential recursion.

These characteristics provide strong provenance for the reconstructed NSCR
state machine.


## Least-threat description versus recovered movement code

The publication prose states that NSCR can evaluate multiple candidate
positions for a conflicting queen and select a position with the least threat
level.

The recovered C# draft does not contain an explicit routine that scores all
candidate rows and selects a global least-threat row. Its `MoveDown` routine
advances the active queen row-by-row, calls the threat checker after each move,
and reacts to the resulting conflict state.

Because the supplied implementation does not provide the missing scoring rule,
the clean reconstruction does **not** invent one. It preserves the concrete
row-by-row conflict-resolution state machine from the recovered source while
documenting this difference from the publication prose.

Consequently, this repository should be treated as a source-informed NSCR
reference reconstruction, not proof that the exact final unpublished
implementation has been recovered.

## Experimental-driver mismatch

The recovered file benchmarks every n from 8 through 100 in a single loop.

The final paper describes a different experimental presentation, including
larger NSCR problem sizes up to 1000 and comparative experiments that are not
represented by that simple loop.

The repository therefore does not use the draft CSV driver as evidence that it
can reproduce the final published benchmark tables.

## Comparator naming and implementation

The recovered file contains routines labelled:

- Constraint Propagation;
- Lookahead;
- CSP.

The paper discusses NSCR relative to BFC, CSP, Lookahead and other approaches.

Because the supplied comparator harness is not a clean one-to-one match for the
final experimental description, those comparator implementations are not
included in the reference release.

## Six-queen discrepancy

The recovered draft contains:

```text
if (max_queen == 6)
{
    Qy[0] = 2;
}
```

With the file's zero-based row representation, this selects the third row.

The publication describes a six-queen workaround that places the first queen
in the fourth row of the first column.

Those are not equivalent.

The clean API therefore leaves n=6 unresolved by default. The draft behaviour
can be enabled explicitly through `UseHistoricalSixQueenWorkaround`.

## Conflict-pointer discrepancy

Inside the recovered movement routine, a newly discovered conflict performs a
write equivalent to:

```text
ptrConflict[mconflict] = i;
```

where `i` is the movement-loop counter.

Elsewhere, entries in `ptrConflict` are used as queen indices. Mixing a
row-attempt counter with a queen index is inconsistent with that use.

The clean reconstruction records reciprocal queen-column indices instead:

```text
previousConflictColumn[conflict] = queenColumn;
```

This is a documented correctness reconstruction, not a claim that the draft
contained that exact statement.

## Memory measurement

The draft labels the difference between two `GC.GetTotalMemory(true)`
measurements as peak memory usage.

That quantity is not a true process peak-memory measurement: it is a
before/after managed-heap snapshot difference affected by garbage collection.

The clean repository therefore does not reproduce or advertise that metric as
peak memory.

## Global operation counter

The draft has a global `z` counter that accumulates movement operations across
successive problem sizes without being reset in the outer benchmark loop.

That makes its CSV `z` column cumulative rather than an independent
per-problem operation count.

The clean solver returns an operation count per solve instead.

## Reconstruction boundary

The reference library is intended to make the algorithm inspectable and
testable. It is not presented as a forensic recreation of the exact executable
used for the paper's measurements.

Any future attempt to reproduce the paper's numerical benchmark tables should
be treated as a separate replication study.
