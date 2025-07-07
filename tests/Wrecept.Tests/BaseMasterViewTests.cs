using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Tests;

public class BaseMasterViewTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
        Application.Current.Resources["RetroDataGridStyle"] = new Style(typeof(DataGrid));
        Application.Current.Resources["RetroDataGridRowStyle"] = new Style(typeof(DataGridRow));
    }

    private class StubView : BaseMasterView
    {
        public StubView() : base() { }
        public DataGrid ExposedGrid => Grid;
        public void TriggerLoaded()
            => typeof(BaseMasterView).GetMethod("OnLoaded", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(this, new object[] { this, new RoutedEventArgs() });
    }

    [StaFact]
    public void OnLoaded_CopiesColumnsAndTemplate()
    {
        EnsureApp();
        var view = new StubView();
        var col1 = new DataGridTextColumn();
        var col2 = new DataGridTextColumn();
        view.Columns.Add(col1);
        view.Columns.Add(col2);
        var template = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(TextBlock)) };
        view.RowDetailsTemplate = template;

        view.TriggerLoaded();

        Assert.Equal(2, view.ExposedGrid.Columns.Count);
        Assert.Same(template, view.ExposedGrid.RowDetailsTemplate);
    }
}
