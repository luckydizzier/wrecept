from decimal import Decimal

from facturon_py.services.invoice_service import InvoiceService


def test_single_item_totals() -> None:
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
    assert invoice.net_total == Decimal("1000.00")
    assert invoice.vat_total == Decimal("270.00")
    assert invoice.gross_total == Decimal("1270.00")
    assert invoice.items[0].vat_rate == Decimal("27")
