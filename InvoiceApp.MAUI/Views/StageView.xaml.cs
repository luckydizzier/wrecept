using InvoiceApp.MAUI.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.MAUI.Views;

public partial class StageView : ContentView
{
    public StageView() : this(MauiProgram.Services!.GetRequiredService<StageViewModel>())
    {
    }

    public StageView(StageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
