using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
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
        ViewModel.ShowMessageRequested += msg => MessageBox.Show(msg, "Névjegy");
        DataContext = ViewModel;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        MainMenu.FirstButton.Focus();
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
                MainMenu.FirstButton.Focus();
            }
        }
    }

    private void SubmenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            ViewModel.SelectedSubmenuIndex = Convert.ToInt32(btn.Tag);
            ViewModel.ExecuteCurrentSubmenu();
            ViewModel.IsSubMenuOpen = false;
        }
    }
}
