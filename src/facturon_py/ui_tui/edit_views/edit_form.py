from __future__ import annotations

from typing import Any, Callable, Dict

from prompt_toolkit.shortcuts import confirm


class EditFormController:
    """Controller for editing forms with dirty tracking."""

    def __init__(
        self,
        data: Dict[str, Any],
        confirm_discard: Callable[[str], bool] | None = None,
    ) -> None:
        self._initial = data.copy()
        self.data = data.copy()
        self._confirm = confirm_discard or (lambda msg: confirm(msg))

    def is_dirty(self) -> bool:
        """Return True if the form has unsaved changes."""
        return self.data != self._initial

    def update_field(self, name: str, value: Any) -> None:
        self.data[name] = value

    def revert(self) -> None:
        self.data = self._initial.copy()

    def handle_key(self, key: str) -> bool:
        """Handle key input.

        Returns True when the caller should exit edit mode.
        """
        if key != "escape":
            return False
        if not self.is_dirty():
            return True
        if self._confirm("Discard changes? Y/N"):
            self.revert()
            return True
        return False
