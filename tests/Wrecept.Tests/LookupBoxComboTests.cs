using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Tests;

public class LookupBoxComboTests
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
        var lookup = new LookupBox();
        var text = (TextBox)typeof(LookupBox).GetField("PART_TextBox", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(lookup)!;
        Assert.NotNull(text);
    }
}
