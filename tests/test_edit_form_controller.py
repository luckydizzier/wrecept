from __future__ import annotations

from facturon_py.repo import audit_log
from facturon_py.ui_tui import shortcuts
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


def test_toggle_active_confirmed_logs_action_and_allows_undo():
    audit_log.clear()

    controller = EditFormController({"active": True}, confirm_discard=lambda _msg: True)
    controller.toggle_active()
    assert controller.data["active"] is False
    log = audit_log.get_log()
    assert log[-1]["action"] == "toggle_active"
    assert log[-1]["details"] == {"from": True, "to": False}

    controller.handle_key("c-z")
    assert controller.data["active"] is True
    assert audit_log.get_log()[-1]["action"] == "undo"


def test_toggle_active_decline_does_nothing():
    audit_log.clear()

    controller = EditFormController(
        {"active": False}, confirm_discard=lambda _msg: False
    )
    controller.toggle_active()
    assert controller.data["active"] is False
    assert audit_log.get_log() == []


def test_f1_shows_shortcuts(monkeypatch):
    shown: dict[str, str] = {}

    def fake_show(view: str) -> None:
        shown["view"] = view

    monkeypatch.setattr(shortcuts, "show", fake_show)

    controller = EditFormController({})
    assert controller.handle_key("f1") is False
    assert shown["view"] == "edit_form"
