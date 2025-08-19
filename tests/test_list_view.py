from __future__ import annotations

from prompt_toolkit.input import create_pipe_input
from prompt_toolkit.output.defaults import DummyOutput

from facturon_py.ui_tui.list_view import ListView, Row


def _make_view(rows: list[Row]) -> ListView:
    def provider() -> list[Row]:
        return rows

    return ListView(provider)


def test_navigation_opens_selected_row():
    rows = [Row("A1", "Alpha", "0"), Row("B2", "Beta", "0")]
    view = _make_view(rows)
    with create_pipe_input() as pipe:
        pipe.send_text("\x1b[B")  # down
        pipe.send_text("\r")  # enter
        action, row = view.run(input=pipe, output=DummyOutput())
    assert action == "open"
    assert row == rows[1]


def test_search_filters_rows():
    rows = [
        Row("A1", "Alpha", "0"),
        Row("B2", "Beta", "0"),
        Row("C3", "Gamma", "0"),
    ]
    view = _make_view(rows)
    with create_pipe_input() as pipe:
        pipe.send_text("\x06Beta\n")  # Ctrl+F then query and Enter
        pipe.send_text("\r")
        action, row = view.run(input=pipe, output=DummyOutput())
    assert action == "open"
    assert row == rows[1]
