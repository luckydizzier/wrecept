using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace InvoiceApp.MAUI.ViewModels;

public abstract partial class MasterDataBaseViewModel<T> : ObservableObject, IMasterDataViewModel
{
    public ObservableCollection<T> Items { get; } = new();

    [ObservableProperty]
    private T? selectedItem;

    partial void OnSelectedItemChanged(T? value)
        => SelectedItemChanged(value);

    protected virtual void SelectedItemChanged(T? value) { }

    public virtual async Task LoadAsync()
    {
        var items = await GetItemsAsync();
        Items.Clear();
        foreach (var item in items)
            Items.Add(item);
    }

    protected abstract Task<List<T>> GetItemsAsync();
}
