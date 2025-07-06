using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.Views.InlinePrompts;

public partial class DeleteItemPromptView : UserControl
{
    private readonly FocusManager _focus;

    public DeleteItemPromptView() : this(App.Provider.GetRequiredService<FocusManager>())
    {
    }

    public DeleteItemPromptView(FocusManager focus)
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
