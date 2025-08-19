from __future__ import annotations

from prompt_toolkit.input import create_pipe_input
from prompt_toolkit.output.defaults import DummyOutput

from facturon_py.ui_tui.main_menu import run_main_menu
from facturon_py.ui_tui import shortcuts


def _run_with_keys(*keys: str) -> str:
    """Helper to run the menu feeding given key sequences."""
    with create_pipe_input() as pipe:
        for key in keys:
            pipe.send_text(key)
        return run_main_menu(input=pipe, output=DummyOutput())


def test_select_invoices():
    # Alt+1 should select invoices
    result = _run_with_keys("\x1b1")
    assert result == "invoices"


def test_exit_with_alt_0():
    result = _run_with_keys("\x1b0")
    assert result == "exit"


def test_f1_shows_shortcuts(monkeypatch):
    called: dict[str, str] = {}

    def fake_show(view: str) -> None:  # pragma: no cover - trivial
        called["view"] = view

    monkeypatch.setattr(shortcuts, "show", fake_show)

    result = _run_with_keys("\x1bOP", "\x1b0")
    assert called["view"] == "main_menu"
    assert result == "exit"
