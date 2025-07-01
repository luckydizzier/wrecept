using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Wrecept.Wpf.ViewModels;

public abstract partial class MasterDataBaseViewModel<T> : ObservableObject
{
    public ObservableCollection<T> Items { get; } = new();

    [ObservableProperty]
    private T? selectedItem;

    public async Task LoadAsync()
    {
        var items = await GetItemsAsync();
        Items.Clear();
        foreach (var item in items)
            Items.Add(item);
    }

    protected abstract Task<List<T>> GetItemsAsync();
}
