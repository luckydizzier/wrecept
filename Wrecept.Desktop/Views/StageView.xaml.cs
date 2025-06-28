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

    private void MainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            ViewModel.SelectedIndex = Convert.ToInt32(btn.Tag);
            ViewModel.IsSubMenuOpen = true;
            ViewModel.SelectedSubmenuIndex = 0;
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
