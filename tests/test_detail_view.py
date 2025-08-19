from __future__ import annotations

from prompt_toolkit.input import create_pipe_input
from prompt_toolkit.output.defaults import DummyOutput

from facturon_py.ui_tui.detail_view import DetailView
from facturon_py.ui_tui.edit_views.edit_form import EditFormController


def _make_view(controller: EditFormController | None = None) -> DetailView:
    header = ["H1", "H2"]
    body = ["B1", "B2", "B3"]
    related_lines = ["R1", "R2"]
    meta = ["M1"]

    def related_provider() -> list[str]:
        return list(related_lines)

    return DetailView(
        header,
        body,
        related_provider,
        meta,
        controller=controller,
    )


def test_tab_and_arrow_navigation():
    view = _make_view()
    with create_pipe_input() as pipe:
        pipe.send_text("\t")  # to body
        pipe.send_text("\x1b[B")  # down -> index 1
        pipe.send_text("\t")  # to related
        pipe.send_text("\x1b[B")  # down -> index 1
        pipe.send_text("\t")  # to meta
        pipe.send_text("\x1b[A")  # up -> stays 0
        pipe.send_text("\x1b")  # exit
        action = view.run(input=pipe, output=DummyOutput())
    assert action == "back"
    assert view.section_index == 3
    assert view.line_index == 0


def test_escape_dirty_uses_edit_controller():
    responses = [False, True]

    def confirm(_msg: str) -> bool:
        return responses.pop(0)

    controller = EditFormController({"name": "Alice"}, confirm_discard=confirm)
    controller.update_field("name", "Bob")
    view = _make_view(controller)

    with create_pipe_input() as pipe:
        pipe.send_text("\x1b")  # first attempt, decline
        pipe.send_text("\x1b")  # second attempt, confirm
        action = view.run(input=pipe, output=DummyOutput())
    assert action == "back"
    assert responses == []
    assert controller.data == {"name": "Alice"}
