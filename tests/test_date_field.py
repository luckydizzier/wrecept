from __future__ import annotations

import pytest
from prompt_toolkit.document import Document
from prompt_toolkit.validation import ValidationError

from facturon_py.ui_tui.widgets.date_field import DateField


def test_normalize_en_us():
    field = DateField("en_US")
    assert field.normalize("12/31/2024") == "2024-12-31"


def test_normalize_de_de():
    field = DateField("de_DE")
    assert field.normalize("31.12.2024") == "2024-12-31"


def test_validator_rejects_bad_format():
    field = DateField("en_US")
    validator = field.validator()
    with pytest.raises(ValidationError):
        validator.validate(Document("31/12/2024"))


def test_prompt_localized():
    assert DateField("de_DE").prompt().startswith("Enter date (DD.MM.YYYY)")
