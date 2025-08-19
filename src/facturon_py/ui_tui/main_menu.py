"""Main menu for the TUI application.

The menu supports Alt+digit navigation and exposes the available shortcuts via
``shortcuts.show`` when the user presses F1.
"""

from __future__ import annotations

from typing import List, Tuple

from prompt_toolkit.application import Application
from prompt_toolkit.input import Input
from prompt_toolkit.input.defaults import DummyInput
from prompt_toolkit.key_binding import KeyBindings
from prompt_toolkit.layout import Layout
from prompt_toolkit.layout.containers import HSplit, Window
from prompt_toolkit.layout.controls import FormattedTextControl
from prompt_toolkit.output import Output
from prompt_toolkit.output.defaults import DummyOutput

from . import shortcuts

# (key, label, identifier)
_MENU: List[Tuple[str, str, str]] = [
    ("1", "Számlák", "invoices"),
    ("2", "Termékek", "products"),
    ("3", "Vevők", "customers"),
    ("4", "Szállítók", "suppliers"),
    ("5", "Adókódok", "tax_codes"),
    ("6", "Áfakulcsok", "tax_rates"),
    ("7", "Beállítások", "settings"),
    ("0", "Kilépés", "exit"),
]

# Register shortcuts so F1 can display them
shortcuts.register(
    "main_menu",
    [(f"Alt+{key}", label) for key, label, _ in _MENU] + [("F1", "Gyorsbillentyűk")],
)


def _build_app(
    *, input: Input | None = None, output: Output | None = None
) -> Application:
    """Create the prompt_toolkit application for the main menu."""

    kb = KeyBindings()

    for key, _label, ident in _MENU:

        @kb.add("escape", key)
        def _handler(event, ident=ident) -> None:  # type: ignore[no-redef]
            event.app.exit(result=ident)

    @kb.add("f1")
    def _show_shortcuts(event) -> None:  # pragma: no cover - trivial
        shortcuts.show("main_menu")

    text = "\n".join(f"{key}. {label}" for key, label, _ in _MENU)
    body = HSplit([Window(FormattedTextControl(text), always_hide_cursor=True)])

    return Application(
        layout=Layout(body),
        key_bindings=kb,
        mouse_support=False,
        full_screen=False,
        input=input or DummyInput(),
        output=output or DummyOutput(),
    )


def run_main_menu(*, input: Input | None = None, output: Output | None = None) -> str:
    """Run the main menu and return the selected identifier."""

    app = _build_app(input=input, output=output)
    return app.run()
