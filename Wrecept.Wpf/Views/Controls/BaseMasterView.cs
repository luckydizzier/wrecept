using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        dataGrid.RowDetailsVisibilityChanged += Grid_RowDetailsVisibilityChanged;
        grid.Children.Add(dataGrid);

        var hint = new TextBlock
        {
            Text = "[Enter] Szerkesztés  [Del] Törlés  [Esc] Vissza",
            Margin = new Thickness(4),
            HorizontalAlignment = HorizontalAlignment.Center
        };
        System.Windows.Controls.Grid.SetRow(hint, 1);
        grid.Children.Add(hint);

        Content = grid;
        return dataGrid;
    }

    protected void InitializeViewModel(IMasterDataViewModel viewModel)
    {
        DataContext = viewModel;
        Loaded += async (_, _) =>
        {
            await viewModel.LoadAsync();
            Grid.Focus();
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



    private void Grid_RowDetailsVisibilityChanged(object? sender, DataGridRowDetailsEventArgs e)
    {
        if (e.DetailsElement.FindName("InitialFocus") is Control box &&
            e.Row.DetailsVisibility == Visibility.Visible)
            box.Focus();
        else if (e.Row.DetailsVisibility != Visibility.Visible)
            Grid.Focus();
    }
}
