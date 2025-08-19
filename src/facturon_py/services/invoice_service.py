from __future__ import annotations

from dataclasses import dataclass, field
from decimal import Decimal, ROUND_HALF_EVEN
from typing import Iterable, Mapping


TWOPLACES = Decimal("0.01")


def _q(value: Decimal) -> Decimal:
    return value.quantize(TWOPLACES, ROUND_HALF_EVEN)


@dataclass
class InvoiceItem:
    description: str
    quantity: Decimal
    unit_price: Decimal
    vat_rate: Decimal  # percent

    @property
    def net(self) -> Decimal:
        return _q(self.quantity * self.unit_price)

    @property
    def vat(self) -> Decimal:
        return _q(self.net * self.vat_rate / Decimal("100"))

    @property
    def gross(self) -> Decimal:
        return self.net + self.vat


@dataclass
class Invoice:
    customer: str
    supplier: str
    issue_date: str
    due_date: str
    items: list[InvoiceItem] = field(default_factory=list)
    net_total: Decimal = Decimal("0")
    vat_total: Decimal = Decimal("0")
    gross_total: Decimal = Decimal("0")


class InvoiceService:
    """Create invoices with VAT snapshot and totals."""

    def __init__(self, tax_rates: Mapping[str, Decimal]):
        self._tax_rates = tax_rates

    def create_invoice(
        self,
        *,
        customer: str,
        supplier: str,
        issue_date: str,
        due_date: str,
        items: Iterable[Mapping[str, Decimal | str]],
    ) -> Invoice:
        line_items: list[InvoiceItem] = []
        for data in items:
            vat_code = str(data["vat_code"])
            vat_rate = self._tax_rates[vat_code]
            item = InvoiceItem(
                description=str(data["description"]),
                quantity=Decimal(str(data["quantity"])),
                unit_price=Decimal(str(data["unit_price"])),
                vat_rate=vat_rate,
            )
            line_items.append(item)

        net_total = _q(sum((item.net for item in line_items), Decimal("0")))
        vat_total = _q(sum((item.vat for item in line_items), Decimal("0")))
        gross_total = _q(sum((item.gross for item in line_items), Decimal("0")))

        return Invoice(
            customer=customer,
            supplier=supplier,
            issue_date=issue_date,
            due_date=due_date,
            items=line_items,
            net_total=net_total,
            vat_total=vat_total,
            gross_total=gross_total,
        )


__all__ = ["InvoiceService", "Invoice", "InvoiceItem"]
