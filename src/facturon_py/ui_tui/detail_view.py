from __future__ import annotations

from typing import Callable, Sequence

from prompt_toolkit.application import Application
from prompt_toolkit.input import Input
from prompt_toolkit.input.defaults import DummyInput
from prompt_toolkit.key_binding import KeyBindings
from prompt_toolkit.layout import Layout
from prompt_toolkit.layout.containers import Window
from prompt_toolkit.layout.controls import FormattedTextControl
from prompt_toolkit.output import Output
from prompt_toolkit.output.defaults import DummyOutput

from . import shortcuts
from .edit_views.edit_form import EditFormController


class DetailView:
    """Simple detail view with section navigation and edit support.

    Parameters
    ----------
    header:
        Lines shown in the *Header* section.
    body:
        Lines shown in the *Body* section.
    related_provider:
        Callable returning lines for the *Related records* section. This allows
        embedding tables such as invoice line items.
    meta:
        Lines shown in the *Meta* section.
    controller:
        Optional :class:`EditFormController` used for dirty checks when leaving
        the view.
    view_id:
        Identifier used for shortcut registration.
    """

    _SECTION_NAMES = ["Fejléc", "Törzs", "Kapcsolódó", "Meta"]

    def __init__(
        self,
        header: Sequence[str],
        body: Sequence[str],
        related_provider: Callable[[], Sequence[str]],
        meta: Sequence[str],
        *,
        controller: EditFormController | None = None,
        view_id: str = "detail_view",
    ) -> None:
        self._sections = [
            list(header),
            list(body),
            list(related_provider()),
            list(meta),
        ]
        self._related_provider = related_provider
        self.section_index = 0
        self.line_index = 0
        self.controller = controller
        shortcuts.register(
            view_id,
            [
                ("Tab", "Szakasz váltás"),
                ("↑/↓", "Mozgás"),
                ("F2", "Szerkeszt"),
                ("Esc", "Vissza"),
            ],
        )

    # ------------------------------------------------------------------
    def _current_lines(self) -> list[str]:
        if self.section_index == 2:  # refresh related records
            self._sections[2] = list(self._related_provider())
        return self._sections[self.section_index]

    def _render(self) -> str:
        lines = self._current_lines()
        header = f"[{self._SECTION_NAMES[self.section_index]}]"
        if not lines:
            return header + "\nNincs adat."
        rendered = [
            f"{'>' if i == self.line_index else ' '} {line}"
            for i, line in enumerate(lines)
        ]
        return "\n".join([header] + rendered)

    # ------------------------------------------------------------------
    def _build_app(
        self, *, input: Input | None = None, output: Output | None = None
    ) -> Application:
        kb = KeyBindings()

        @kb.add("tab")
        def _next_section(event) -> None:  # pragma: no cover - simple
            self.section_index = (self.section_index + 1) % len(self._sections)
            self.line_index = 0
            event.app.invalidate()

        @kb.add("up")
        def _up(event) -> None:  # pragma: no cover - simple
            if self.line_index > 0:
                self.line_index -= 1
                event.app.invalidate()

        @kb.add("down")
        def _down(event) -> None:  # pragma: no cover - simple
            if self.line_index < len(self._current_lines()) - 1:
                self.line_index += 1
                event.app.invalidate()

        @kb.add("f2")
        def _edit(event) -> None:
            event.app.exit(result="edit")

        @kb.add("escape")
        def _back(event) -> None:
            if self.controller and not self.controller.handle_key("escape"):
                return
            event.app.exit(result="back")

        body = Window(
            FormattedTextControl(lambda: self._render()), always_hide_cursor=True
        )

        return Application(
            layout=Layout(body),
            key_bindings=kb,
            mouse_support=False,
            full_screen=False,
            input=input or DummyInput(),
            output=output or DummyOutput(),
        )

    # ------------------------------------------------------------------
    def run(self, *, input: Input | None = None, output: Output | None = None) -> str:
        """Run the detail view and return the action."""
        app = self._build_app(input=input, output=output)
        return app.run()


__all__ = ["DetailView"]
