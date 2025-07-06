using System.Windows;
using System.Windows.Controls;
using Wrecept.Wpf;
using Xunit;

namespace Wrecept.Tests;

public class VisualTreeExtensionsTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void FindAncestor_ReturnsAncestor()
    {
        EnsureApp();
        var grid = new Grid();
        var border = new Border();
        var box = new TextBox();
        border.Child = box;
        grid.Children.Add(border);

        var ancestor = box.FindAncestor<Grid>();

        Assert.Same(grid, ancestor);
    }

    [StaFact]
    public void FindAncestor_ReturnsNull_WhenNotFound()
    {
        EnsureApp();
        var grid = new Grid();
        var border = new Border();
        var box = new TextBox();
        border.Child = box;
        grid.Children.Add(border);

        var result = box.FindAncestor<Window>();

        Assert.Null(result);
    }
}
