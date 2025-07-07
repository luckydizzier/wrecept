using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xunit;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Tests;

public class EditLookupTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [Fact]
    public void AutoCompleteBox_HasContainsFilter()
    {
        EnsureApp();
        var lookup = new EditLookup();
        var box = (AutoCompleteBox)typeof(EditLookup).GetField("Box", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(lookup)!;
        Assert.Equal(AutoCompleteFilterMode.Contains, box.FilterMode);
    }
}
