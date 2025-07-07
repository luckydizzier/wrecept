using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class InvoiceCreatePromptView : UserControl
{
    private readonly FocusManager _focus;

    public InvoiceCreatePromptView() : this(App.Provider.GetRequiredService<FocusManager>())
    {
    }

    public InvoiceCreatePromptView(FocusManager focus)
    {
        _focus = focus;
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        _focus.RequestFocus(NumberBox);
    }

}
