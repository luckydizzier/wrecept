using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class SaveLinePromptView : UserControl
{
    public SaveLinePromptView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        Focus();
    }

}

