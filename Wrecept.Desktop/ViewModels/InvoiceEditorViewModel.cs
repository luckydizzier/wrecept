using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Wrecept.Desktop.ViewModels;

public partial class InvoiceEditorViewModel : ObservableObject
{
    public ObservableCollection<ItemRow> Items { get; } = new();

    public InvoiceEditorViewModel()
    {
        Items.Add(new ItemRow("Tétel 1", 100m, 127m));
    }
}

public record ItemRow(string Name, decimal Net, decimal Gross);
