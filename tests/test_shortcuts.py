from __future__ import annotations

from facturon_py.ui_tui import shortcuts


def test_register_and_get():
    shortcuts.clear()
    shortcuts.register("view", [("A", "Valami")])
    assert shortcuts.get("view") == [("A", "Valami")]


def test_show_uses_message_dialog(monkeypatch):
    shortcuts.clear()
    shortcuts.register("view", [("A", "Valami")])

    recorded: dict[str, str] = {}

    class DummyDialog:
        def __init__(self, title: str, text: str) -> None:  # noqa: D401 - simple
            recorded["title"] = title
            recorded["text"] = text

        def run(self) -> None:  # pragma: no cover - no behavior needed
            recorded["run"] = "yes"

    monkeypatch.setattr(shortcuts, "message_dialog", DummyDialog)

    shortcuts.show("view")

    assert recorded["title"] == "Gyorsbillentyűk"
    assert "A - Valami" in recorded["text"]
    assert recorded["run"] == "yes"
