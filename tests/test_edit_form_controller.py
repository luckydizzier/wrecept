from __future__ import annotations

from facturon_py.ui_tui.edit_views.edit_form import EditFormController


def test_escape_without_dirty_exits_without_prompt():
    prompted = False

    def confirm(_msg: str) -> bool:  # pragma: no cover - should not be called
        nonlocal prompted
        prompted = True
        return True

    controller = EditFormController({"name": "Alice"}, confirm_discard=confirm)
    assert controller.handle_key("escape") is True
    assert prompted is False


def test_escape_dirty_and_confirm_reverts_and_exits():
    controller = EditFormController(
        {"name": "Alice"}, confirm_discard=lambda _msg: True
    )
    controller.update_field("name", "Bob")
    assert controller.is_dirty() is True
    assert controller.handle_key("escape") is True
    assert controller.data == {"name": "Alice"}


def test_escape_dirty_and_decline_stays_in_edit_mode():
    controller = EditFormController(
        {"name": "Alice"}, confirm_discard=lambda _msg: False
    )
    controller.update_field("name", "Bob")
    assert controller.handle_key("escape") is False
    assert controller.data == {"name": "Bob"}
