# Validation status

## Completed checks

The reconstruction has been checked for:

- valid N-Queens solutions on representative solvable sizes;
- valid solutions at larger sizes through n=1000;
- correct unsolved status for n=2 and n=3;
- deterministic output for repeated runs at the same n;
- explicit handling of the unresolved six-queen discrepancy;
- successful historical-draft six-queen mode;
- final board validation independent of the solver state machine;
- invalid-input handling;
- operation-limit handling;
- project structure suitable for automated .NET build testing.

## CI

GitHub Actions is configured to:

1. restore the solution;
2. build it in Release mode;
3. run the smoke-test executable;

on both Windows and Linux.

## Evidence boundary

Passing the smoke tests means the reconstruction returns valid boards for the
tested cases. It does **not** establish that:

- the implementation is byte-for-byte identical to the software used in 2024;
- runtime or memory figures reproduce the paper;
- every n up to 1000 follows exactly the same state path as the unpublished
  experimental implementation;
- the paper's complexity analysis has been independently proved by the tests.

## Benchmark validation

No benchmark comparison is published in this repository yet.

If benchmarking is added later, it should report:

- runtime environment;
- .NET version;
- operating system;
- CPU;
- warm-up policy;
- repetition count;
- aggregation statistic;
- memory metric definition;
- exact input sizes;
- validation that each produced board is correct.

New benchmark results should be clearly labelled as new measurements rather
than copied or implied reproductions of the 2024 tables.
