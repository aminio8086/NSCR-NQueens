# Changelog

All notable changes to this research-software release will be documented here.

## [Unreleased]

## [1.0.0] - 2026-10-17

### Added

- Clean C# reference reconstruction of the NSCR state machine.
- O(n)-sized queen, conflict and placement state.
- Explicit N-Queens solution validator.
- Command-line interface.
- Operation-limit guard for reconstruction safety.
- Explicit handling of the six-queen paper/draft discrepancy.
- Smoke tests for representative problem sizes and deterministic behaviour.
- Windows and Linux GitHub Actions CI.
- Build, algorithm, provenance, validation and source-audit documentation.
- Machine-readable citation metadata.
- Apache-2.0 licensing metadata.

### Changed

- Replaced prototype identifiers such as `Qy`, `qm`, `ptr_move` and
  `ptr_place` with descriptive public-release names.
- Separated NSCR solving logic from CSV benchmarking and console reporting.
- Corrected reciprocal conflict bookkeeping in the clean reconstruction;
  the historical draft mixed a movement-loop index with a queen index.
- Added an explicit operation guard rather than allowing an accidental
  non-terminating reconstruction to run indefinitely.

### Not reproduced

- Historical CSV benchmark output.
- Draft memory measurement based on before/after managed-heap snapshots.
- Claims that new measurements reproduce the paper's published tables.
