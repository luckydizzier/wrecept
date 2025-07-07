using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Tests;

public class LookUpEditTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [Fact]
    public void ComboBox_IsEditable()
    {
        EnsureApp();
        var lookup = new LookUpEdit();
        var box = (ComboBox)typeof(LookUpEdit).GetField("Box", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(lookup)!;
        Assert.True(box.IsEditable);
    }
}
