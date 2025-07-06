using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class SaveLinePromptView : UserControl
{
    private readonly FocusManager _focus;

    public SaveLinePromptView() : this(App.Provider.GetRequiredService<FocusManager>())
    {
    }

    public SaveLinePromptView(FocusManager focus)
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

