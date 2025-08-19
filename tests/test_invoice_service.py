from decimal import Decimal

from facturon_py.services.invoice_service import (
    Invoice,
    InvoiceItem,
    InvoiceService,
    _build_line_items,
    _summarize_totals,
)


def test_build_line_items() -> None:
    items = [
        {
            "description": "Service",
            "quantity": Decimal("1"),
            "unit_price": Decimal("1000"),
            "vat_code": "27",
        }
    ]
    line_items = _build_line_items(items, {"27": Decimal("27")})
    assert len(line_items) == 1
    assert isinstance(line_items[0], InvoiceItem)
    assert line_items[0].vat_rate == Decimal("27")


def test_summarize_totals() -> None:
    line_items = [
        InvoiceItem("A", Decimal("1"), Decimal("100"), Decimal("27")),
        InvoiceItem("B", Decimal("2"), Decimal("50"), Decimal("5")),
    ]
    net_total, vat_total, gross_total = _summarize_totals(line_items)
    assert net_total == Decimal("200.00")
    assert vat_total == Decimal("32.00")
    assert gross_total == Decimal("232.00")


def test_create_invoice_single_item_totals() -> None:
    service = InvoiceService({"27": Decimal("27")})
    invoice = service.create_invoice(
        customer="Alice",
        supplier="ACME",
        issue_date="2025-08-19",
        due_date="2025-08-20",
        items=[
            {
                "description": "Service",
                "quantity": Decimal("1"),
                "unit_price": Decimal("1000"),
                "vat_code": "27",
            }
        ],
    )
    assert isinstance(invoice, Invoice)
    assert invoice.net_total == Decimal("1000.00")
    assert invoice.vat_total == Decimal("270.00")
    assert invoice.gross_total == Decimal("1270.00")
    assert invoice.items[0].vat_rate == Decimal("27")
