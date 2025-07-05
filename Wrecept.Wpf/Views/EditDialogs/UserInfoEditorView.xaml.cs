using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.Services;
using FocusService = Wrecept.Wpf.Services.FocusManager;

namespace Wrecept.Wpf.Views.EditDialogs;

public partial class UserInfoEditorView : UserControl
{
    public UserInfoEditorView()
    {
        InitializeComponent();
    }
}
