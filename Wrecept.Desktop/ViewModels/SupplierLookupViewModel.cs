using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;

namespace Wrecept.Desktop.ViewModels;

public partial class SupplierLookupViewModel : ObservableObject
{
    public ObservableCollection<Supplier> Suppliers { get; } = new();

    public SupplierLookupViewModel()
    {
        Suppliers.Add(new Supplier { Id = 1, Name = "Teszt Kft." });
        Suppliers.Add(new Supplier { Id = 2, Name = "Minta Bt." });
    }
}
