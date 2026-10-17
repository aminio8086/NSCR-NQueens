# NSCR reconstruction

## Problem representation

The solver stores one queen per column.

For a board of size `n`:

```text
rowsByColumn[column] = row
```

A solution is valid when no two columns contain queens on the same row or on
either diagonal.

## Recovered NSCR state

The historical C# draft uses three O(n)-sized collections:

- queen row positions;
- previous/conflicting queen pointers;
- placement-state flags.

The clean reconstruction preserves that state model with descriptive names.

## Conflict check

For a queen in column `c`, another placed queen in column `i` conflicts
when at least one of the following is true:

```text
row[i] == row[c]
i - row[i] == c - row[c]
i + row[i] == c + row[c]
```

The recovered draft returns:

- `-1` for no conflict;
- a queen-column index for one conflict;
- `-2` for multiple conflicts.

The clean implementation retains this three-state behaviour.

## Non-sequential conflict resolution

When the current queen conflicts with a previously placed queen, the solver
records the relationship and may redirect work to the conflicting queen rather
than simply continuing a conventional left-to-right recursive backtrack.

The active queen is then moved through candidate rows until the state machine
finds a non-conflicting placement, detects a different conflict, or cycles back
to its starting row.

This conflict redirection is the central behaviour retained from the recovered
NSCR code.

## Space

The reference implementation stores a constant number of arrays proportional
to `n`, plus scalar state. It therefore uses O(n) algorithmic auxiliary
storage, excluding returned result objects and runtime/framework overhead.

## Complexity statement

The associated paper reports O(n) space complexity and discusses a worst-case
time complexity up to O(n³).

This repository does not attempt to prove that bound independently. It records
the paper's claim and provides an implementation whose state representation is
consistent with linear auxiliary storage.

## Six-queen ambiguity

The recovered draft and paper disagree about the special starting position for
n=6. See `source-audit.md`.

The default API treats that discrepancy as unresolved rather than silently
changing either source.
