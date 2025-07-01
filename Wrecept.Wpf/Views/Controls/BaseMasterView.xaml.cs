using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views.Controls;

public abstract partial class BaseMasterView : UserControl
{
    public ObservableCollection<DataGridColumn> Columns { get; } = new();

    public DataTemplate? RowDetailsTemplate { get; set; }

    protected BaseMasterView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    protected void InitializeViewModel(IMasterDataViewModel viewModel)
    {
        DataContext = viewModel;
        Loaded += async (_, _) =>
        {
            await viewModel.LoadAsync();
            Grid.Focus();
        };
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Columns.Count > 0 && Grid.Columns.Count == 0)
            foreach (var c in Columns)
                Grid.Columns.Add(c);

        if (RowDetailsTemplate != null && Grid.RowDetailsTemplate == null)
            Grid.RowDetailsTemplate = RowDetailsTemplate;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);

    private void Grid_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
    {
        if (e.DetailsElement.FindName("InitialFocus") is Control box &&
            e.Row.DetailsVisibility == Visibility.Visible)
            box.Focus();
        else if (e.Row.DetailsVisibility != Visibility.Visible)
            Grid.Focus();
    }
}
