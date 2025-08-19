from __future__ import annotations

import datetime as dt
import locale
import re
from dataclasses import dataclass
from typing import Tuple

from prompt_toolkit.validation import ValidationError, Validator

# Mapping of locale codes to (strftime format, regex pattern, display mask)
LOCALE_FORMATS: dict[str, Tuple[str, str, str]] = {
    "en_US": ("%m/%d/%Y", r"\d{2}/\d{2}/\d{4}", "MM/DD/YYYY"),
    "de_DE": ("%d.%m.%Y", r"\d{2}\.\d{2}\.\d{4}", "DD.MM.YYYY"),
}
DEFAULT_FORMAT = ("%Y-%m-%d", r"\d{4}-\d{2}-\d{2}", "YYYY-MM-DD")


def _locale_spec(locale_code: str | None) -> Tuple[str, str, str]:
    """Return format, regex, and mask for the given locale."""
    if not locale_code:
        locale_code = locale.getlocale(locale.LC_TIME)[0] or ""
    return LOCALE_FORMATS.get(locale_code, DEFAULT_FORMAT)


@dataclass
class DateField:
    """Utility for localized date input and normalization."""

    locale_code: str | None = None

    def prompt(self) -> str:
        """Return localized prompt hint."""
        _fmt, _pattern, mask = _locale_spec(self.locale_code)
        return f"Enter date ({mask}):"

    def normalize(self, text: str) -> str:
        """Validate and normalize the input to ISO format."""
        fmt, pattern, mask = _locale_spec(self.locale_code)
        if not re.fullmatch(pattern, text):
            raise ValueError(f"Expected {mask}")
        try:
            parsed = dt.datetime.strptime(text, fmt).date()
        except ValueError as exc:  # pragma: no cover - strptime detail
            raise ValueError(f"Invalid date: {text}") from exc
        return parsed.isoformat()

    def validator(self) -> Validator:
        fmt, pattern, mask = _locale_spec(self.locale_code)

        class _V(Validator):
            def validate(self, document) -> None:  # type: ignore[override]
                text = document.text
                if not re.fullmatch(pattern, text):
                    raise ValidationError(message=f"Expected {mask}")
                try:
                    dt.datetime.strptime(text, fmt)
                except ValueError:
                    raise ValidationError(message="Invalid date")

        return _V()


__all__ = ["DateField"]
