#!/usr/bin/env python3
import pathlib
import re

progress_dir = pathlib.Path('docs/progress')
changelog = pathlib.Path('CHANGELOG.md')

entries = []
for path in sorted(progress_dir.glob('*.md')):
    stamp = path.stem.split('_')[0]
    lines = path.read_text(encoding='utf-8').splitlines()
    bullets = [re.sub(r'^\*\s+', '', l) for l in lines if l.startswith('*')]
    if bullets:
        entries.append((stamp, bullets))

with changelog.open('w', encoding='utf-8') as f:
    f.write('---\n')
    f.write('title: "Changelog"\n')
    f.write('purpose: "Aggregated changes from progress logs"\n')
    f.write('author: "tools"\n')
    f.write('date: "2025-06-27"\n')
    f.write('---\n\n')
    f.write('# 📝 Changelog\n\n')
    for stamp, bullets in entries:
        f.write(f'## {stamp}\n')
        for b in bullets:
            f.write(f'- {b}\n')
        f.write('\n')
