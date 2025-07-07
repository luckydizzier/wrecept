using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wrecept.Wpf.Views.Controls;
using Xunit;

namespace Wrecept.Tests;

public class LookupBoxTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private static Task InvokeFilterAsync(LookupBox lookup)
    {
        var m = typeof(LookupBox).GetMethod("FilterAsync", BindingFlags.Instance | BindingFlags.NonPublic)!;
        return (Task)m.Invoke(lookup, null)!;
    }

    [StaFact]
    public async Task FilterAsync_FiltersAndShowsPrompt()
    {
        EnsureApp();
        var apple = new { Name = "alma" };
        var banana = new { Name = "banán" };
        var lookup = new LookupBox
        {
            ItemsSource = new[] { apple, banana },
            DisplayMemberPath = "Name",
            MaxSuggestions = 10
        };

        lookup.Text = "al";
        await InvokeFilterAsync(lookup);

        Assert.Single(lookup.FilteredItems);
        Assert.Same(apple, lookup.FilteredItems[0]);

        lookup.Text = "xyz";
        await InvokeFilterAsync(lookup);

        Assert.Empty(lookup.FilteredItems);
        var prompt = (TextBlock)typeof(LookupBox).GetField("PART_CreatePrompt", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(lookup)!;
        Assert.Equal(Visibility.Visible, prompt.Visibility);
    }

    [Fact]
    public void Match_ReturnsExpected()
    {
        var item = new { Name = "citrom" };
        var m = typeof(LookupBox).GetMethod("Match", BindingFlags.Static | BindingFlags.NonPublic)!;
        Assert.True((bool)m.Invoke(null, new object[] { item, "tro", "Name" })!);
        Assert.False((bool)m.Invoke(null, new object[] { item, "xyz", "Name" })!);
    }

    [Fact]
    public void GetProperty_ReturnsValue()
    {
        var item = new { Name = "dinnye" };
        var m = typeof(LookupBox).GetMethod("GetProperty", BindingFlags.Static | BindingFlags.NonPublic)!;
        var value = m.Invoke(null, new object[] { item, "Name" });
        Assert.Equal("dinnye", value);
    }
}
