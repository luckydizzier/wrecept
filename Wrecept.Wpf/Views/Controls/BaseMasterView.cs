using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wrecept.Wpf.ViewModels;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf.Views.Controls;

public abstract class BaseMasterView : UserControl
{
    public ObservableCollection<DataGridColumn> Columns { get; } = new();

    public DataTemplate? RowDetailsTemplate { get; set; }

    protected DataGrid Grid { get; }



    protected BaseMasterView()
    {
        Grid = BuildLayout();
        Loaded += OnLoaded;
    }

    private DataGrid BuildLayout()
    {
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var dataGrid = new DataGrid
        {
            Name = "Grid",
            AutoGenerateColumns = false,
            ItemsSource = null,
            Margin = new Thickness(4),
            Style = (Style)Application.Current.Resources["RetroDataGridStyle"],
            RowStyle = (Style)Application.Current.Resources["RetroDataGridRowStyle"],
        };
        grid.Children.Add(dataGrid);

        Content = grid;
        return dataGrid;
    }

    protected void InitializeViewModel(IMasterDataViewModel viewModel)
    {
        DataContext = viewModel;
        Loaded += async (_, _) =>
        {
            await viewModel.LoadAsync();
        };

        BindingOperations.SetBinding(Grid, ItemsControl.ItemsSourceProperty, new Binding("Items"));
        BindingOperations.SetBinding(Grid, DataGrid.SelectedItemProperty, new Binding("SelectedItem") { Mode = BindingMode.TwoWay });
        BindingOperations.SetBinding(Grid, DataGrid.RowDetailsVisibilityModeProperty, new Binding("IsEditing") { Converter = (IValueConverter)Application.Current.Resources["BooleanToRowDetailsConverter"] });
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (Columns.Count > 0 && Grid.Columns.Count == 0)
            foreach (var c in Columns)
                Grid.Columns.Add(c);

        if (RowDetailsTemplate != null && Grid.RowDetailsTemplate == null)
            Grid.RowDetailsTemplate = RowDetailsTemplate;
    }


}
