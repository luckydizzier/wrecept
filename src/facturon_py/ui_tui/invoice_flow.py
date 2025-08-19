from __future__ import annotations

from decimal import Decimal
from typing import Mapping, Sequence

from prompt_toolkit import prompt

from facturon_py.services.invoice_service import Invoice, InvoiceService
from .edit_views.edit_form import EditFormController
from .widgets.date_field import DateField


def new_invoice(
    customers: Sequence[str],
    *,
    supplier: str,
    tax_rates: Mapping[str, Decimal],
) -> Invoice:
    for idx, name in enumerate(customers, 1):
        print(f"{idx}. {name}")
    selected = int(prompt("Select customer: ")) - 1
    customer = customers[selected]

    issue_field = DateField()
    issue_date = issue_field.normalize(prompt(issue_field.prompt()))

    due_field = DateField()
    due_date = due_field.normalize(prompt(due_field.prompt()))

    items = []
    while True:
        controller = EditFormController(
            {
                "description": "",
                "quantity": Decimal("1"),
                "unit_price": Decimal("0"),
                "vat_code": "27",
            }
        )
        controller.update_field("description", prompt("Item description: "))
        controller.update_field("quantity", Decimal(prompt("Quantity: ")))
        controller.update_field("unit_price", Decimal(prompt("Unit price: ")))
        controller.update_field("vat_code", prompt("VAT code: "))
        items.append(controller.data)
        if prompt("Add another item? (y/N): ").lower() != "y":
            break

    service = InvoiceService(tax_rates)
    return service.create_invoice(
        customer=customer,
        supplier=supplier,
        issue_date=issue_date,
        due_date=due_date,
        items=items,
    )


__all__ = ["new_invoice"]
