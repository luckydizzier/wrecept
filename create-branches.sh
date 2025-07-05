#!/bin/sh

git checkout 6ece403 -b release/v0.1.0-alpha

git tag v0.1.0-alpha

# Feature branches
git branch feature/git-branch-policy 5da5aee
git branch feature/appstate-service 4ecb6d2
git branch feature/keyboard-profile 6912c4b
git branch feature/inline-invoice-creator 822aa2e
git branch feature/wpf-shell 94f4310
git branch feature/retro-theme ccdd032
git branch feature/invoice-calculator bbd73b2
git branch feature/database-seeding b7ff573

# Bugfix branches
git branch bugfix/invoiceeditor-focus ab9bf2e
git branch bugfix/invoiceeditor-load 89fdf7e
git branch bugfix/startup-crash f50ba04
git branch bugfix/sqlite-migration 7072c73

# Docs branches
git branch docs/project-structure 9f64789
git branch docs/root-readme b80505e
git branch docs/keyboard-flow 0f780a5
git branch docs/dialog-handling 196b389

# Infra branch
git branch infra/sqlite-dbinit b7ff573

# Tags for main milestones
git tag v0.2.0-alpha dc6cb32
git tag v0.3.0-beta ccdd032
git tag v0.4.0 92fb9d2

echo "Branches and tags created"
