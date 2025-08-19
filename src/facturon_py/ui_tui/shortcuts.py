from __future__ import annotations

from typing import Dict, List, Tuple

from prompt_toolkit.shortcuts import message_dialog

# Registry mapping view identifiers to lists of (key, description)
_registry: Dict[str, List[Tuple[str, str]]] = {}


def register(view: str, shortcuts: List[Tuple[str, str]]) -> None:
    """Register shortcuts for a view."""
    _registry[view] = list(shortcuts)


def get(view: str) -> List[Tuple[str, str]]:
    """Return shortcuts registered for a view."""
    return list(_registry.get(view, []))


def clear() -> None:
    """Clear all registered shortcuts (useful for tests)."""
    _registry.clear()


def show(view: str) -> None:
    """Show shortcuts for the given view in a dialog."""
    lines = [f"{key} - {desc}" for key, desc in get(view)]
    text = "\n".join(lines) if lines else "Nincs gyorsbillentyű."
    message_dialog(title="Gyorsbillentyűk", text=text).run()
