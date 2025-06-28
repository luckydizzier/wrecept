using System.ComponentModel;
using System.Windows.Controls;
using Wrecept.Desktop.ViewModels;

namespace Wrecept.Desktop.Views;

public partial class StageView : UserControl
{
    public StageViewModel ViewModel { get; }


    public StageView()
    {
        InitializeComponent();
        ViewModel = new StageViewModel();
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        DataContext = ViewModel;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        MainMenuFirstButton.Focus();
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StageViewModel.IsSubMenuOpen))
        {
            if (ViewModel.IsSubMenuOpen)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (SubmenuList.ItemContainerGenerator.ContainerFromIndex(0) is FrameworkElement fe)
                        fe.Focus();
                });
            }
            else
            {
                if (MainMenuPanel.Children[ViewModel.SelectedIndex] is Control btn)
                    btn.Focus();
            }
        }
    }
}
