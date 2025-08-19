from __future__ import annotations

from dataclasses import dataclass
from typing import Callable, Sequence, Tuple

from prompt_toolkit.application import Application
from prompt_toolkit.filters import Condition
from prompt_toolkit.input import Input
from prompt_toolkit.input.defaults import DummyInput
from prompt_toolkit.key_binding import KeyBindings
from prompt_toolkit.layout import Layout
from prompt_toolkit.layout.containers import ConditionalContainer, VSplit, Window
from prompt_toolkit.layout.controls import FormattedTextControl
from prompt_toolkit.output import Output
from prompt_toolkit.output.defaults import DummyOutput
from prompt_toolkit.shortcuts import input_dialog

from . import shortcuts


@dataclass
class Row:
    """Data row displayed by :class:`ListView`."""

    code: str
    name: str
    metrics: str
    active: bool = True


class ListView:
    """Simple list view with search and navigation."""

    def __init__(
        self,
        data_provider: Callable[[], Sequence[Row]],
        *,
        view_id: str = "list_view",
    ) -> None:
        self._provider = data_provider
        self.view_id = view_id
        self.show_info = False
        self.refresh()
        shortcuts.register(
            view_id,
            [
                ("↑/↓", "Navigate"),
                ("Enter", "Open"),
                ("Ins", "New"),
                ("F2", "Edit"),
                ("Del", "Deactivate"),
                ("Ctrl+F", "Search"),
                ("→", "Info"),
                ("Esc", "Back"),
            ],
        )

    # ------------------------------------------------------------------
    # Data management
    def refresh(self) -> None:
        """Reload rows from the data provider."""
        self._rows = list(self._provider())
        self._filtered = self._rows
        self._index = 0

    # ------------------------------------------------------------------
    # Rendering helpers
    def _render_rows(self) -> str:
        if not self._filtered:
            return "Nincs adat."
        lines = []
        for idx, row in enumerate(self._filtered):
            marker = ">" if idx == self._index else " "
            status = "" if row.active else " (inaktív)"
            lines.append(
                f"{marker} {row.code:<10} {row.name:<20} {row.metrics:<10}{status}"
            )
        return "\n".join(lines)

    def _render_info(self) -> str:
        if not self._filtered:
            return ""
        row = self._filtered[self._index]
        return f"Kód: {row.code}\nNév: {row.name}\nMérték: {row.metrics}\nAktív: {row.active}"

    # ------------------------------------------------------------------
    # Application setup
    def _build_app(
        self, *, input: Input | None = None, output: Output | None = None
    ) -> Application:
        kb = KeyBindings()

        @kb.add("up")
        def _up(event) -> None:  # pragma: no cover - simple
            if self._index > 0:
                self._index -= 1
                event.app.invalidate()

        @kb.add("down")
        def _down(event) -> None:  # pragma: no cover - simple
            if self._index < len(self._filtered) - 1:
                self._index += 1
                event.app.invalidate()

        @kb.add("enter")
        def _open(event) -> None:
            row = self._filtered[self._index] if self._filtered else None
            event.app.exit(result=("open", row))

        @kb.add("insert")
        def _new(event) -> None:
            event.app.exit(result=("new", None))

        @kb.add("f2")
        def _edit(event) -> None:
            row = self._filtered[self._index] if self._filtered else None
            event.app.exit(result=("edit", row))

        @kb.add("delete")
        def _delete(event) -> None:
            row = self._filtered[self._index] if self._filtered else None
            event.app.exit(result=("deactivate", row))

        @kb.add("c-f")
        def _search(event) -> None:
            query = input_dialog(title="Keresés", text="Kifejezés?").run()
            if query:
                q = query.lower()
                self._filtered = [
                    r for r in self._rows if q in r.code.lower() or q in r.name.lower()
                ]
            else:
                self._filtered = self._rows
            self._index = 0
            event.app.invalidate()

        @kb.add("right")
        def _toggle(event) -> None:  # pragma: no cover - simple
            self.show_info = not self.show_info
            event.app.invalidate()

        @kb.add("escape")
        def _back(event) -> None:
            event.app.exit(result=("back", None))

        list_control = FormattedTextControl(lambda: self._render_rows())
        list_window = Window(list_control, always_hide_cursor=True)

        info_control = FormattedTextControl(lambda: self._render_info())
        info_window = ConditionalContainer(
            Window(info_control, width=24, always_hide_cursor=True),
            Condition(lambda: self.show_info),
        )

        body = VSplit([list_window, info_window])

        return Application(
            layout=Layout(body),
            key_bindings=kb,
            mouse_support=False,
            full_screen=False,
            input=input or DummyInput(),
            output=output or DummyOutput(),
        )

    # ------------------------------------------------------------------
    def run(
        self, *, input: Input | None = None, output: Output | None = None
    ) -> Tuple[str, Row | None]:
        """Run the list view and return (action, row)."""
        app = self._build_app(input=input, output=output)
        return app.run()


__all__ = ["Row", "ListView"]
