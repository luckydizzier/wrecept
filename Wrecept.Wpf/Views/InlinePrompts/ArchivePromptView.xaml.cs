using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using FocusService = Wrecept.Wpf.Services.FocusManager;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class ArchivePromptView : UserControl
{
    public ArchivePromptView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        var focus = App.Provider.GetRequiredService<FocusService>();
        focus.RequestFocus(this);
    }

}

