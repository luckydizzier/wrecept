from __future__ import annotations

from typing import Any, Callable, Dict, List, Tuple

from prompt_toolkit.shortcuts import confirm

from facturon_py.repo import audit_log


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
        self._undo_stack: List[Tuple[str, Any]] = []

    def is_dirty(self) -> bool:
        """Return True if the form has unsaved changes."""
        return self.data != self._initial

    def update_field(self, name: str, value: Any) -> None:
        self.data[name] = value

    def revert(self) -> None:
        self.data = self._initial.copy()

    def toggle_active(self) -> None:
        """Toggle the `active` flag with confirmation and audit logging."""
        if self._confirm("Toggle active? Y/N"):
            previous = self.data.get("active", False)
            self._undo_stack.append(("active", previous))
            new_value = not previous
            self.data["active"] = new_value
            audit_log.log_action("toggle_active", {"from": previous, "to": new_value})

    def undo_last(self) -> None:
        """Undo the last reversible action."""
        if not self._undo_stack:
            return
        field, previous = self._undo_stack.pop()
        current = self.data.get(field)
        self.data[field] = previous
        audit_log.log_action("undo", {"field": field, "from": current, "to": previous})

    def handle_key(self, key: str) -> bool:
        """Handle key input.

        Returns True when the caller should exit edit mode.
        """
        if key == "c-z":
            self.undo_last()
            return False
        if key != "escape":
            return False
        if not self.is_dirty():
            return True
        if self._confirm("Discard changes? Y/N"):
            self.revert()
            return True
        return False
