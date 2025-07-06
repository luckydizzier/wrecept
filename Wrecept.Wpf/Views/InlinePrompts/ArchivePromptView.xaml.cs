using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class ArchivePromptView : UserControl
{
    private readonly FocusManager _focus;

    public ArchivePromptView() : this(App.Provider.GetRequiredService<FocusManager>())
    {
    }

    public ArchivePromptView(FocusManager focus)
    {
        _focus = focus;
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        _focus.RequestFocus(this);
    }

}

