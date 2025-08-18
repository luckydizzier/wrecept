from __future__ import annotations

from datetime import datetime
from typing import Any, Dict, List

_log: List[Dict[str, Any]] = []


def log_action(action: str, details: Dict[str, Any]) -> None:
    """Record an audit entry."""
    _log.append({"action": action, "details": details, "timestamp": datetime.utcnow()})


def get_log() -> List[Dict[str, Any]]:
    """Return a copy of the audit log."""
    return list(_log)


def clear() -> None:
    """Clear the audit log (useful for tests)."""
    _log.clear()
