using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

    private static void InvokeOnTextChanged(EditLookup lookup, TextBox box)
    {
        typeof(EditLookup).GetField("_textBox", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(lookup, box);
        var m = typeof(EditLookup).GetMethod("OnTextChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;
        m.Invoke(lookup, new object[] { box, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None) });
    }

    [StaFact]
    public void OnTextChanged_FiltersCollectionView()
    {
        EnsureApp();
        var items = new[] { new { Name = "alma" }, new { Name = "barack" } };
        var cvs = new CollectionViewSource { Source = items };
        var lookup = new EditLookup { ItemsSource = cvs, DisplayMemberPath = "Name" };
        var box = new TextBox { Text = "al" };

        InvokeOnTextChanged(lookup, box);
        var view = CollectionViewSource.GetDefaultView(cvs);
        Assert.Single(view.Cast<object>());
        Assert.True(((ComboBox)typeof(EditLookup).GetField("Box", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(lookup)!).IsDropDownOpen);

        box.Text = "zz";
        InvokeOnTextChanged(lookup, box);
        Assert.Empty(view.Cast<object>());
    }

    [Fact]
    public void Matches_WorksCorrectly()
    {
        var lookup = new EditLookup { DisplayMemberPath = "Name" };
        var m = typeof(EditLookup).GetMethod("Matches", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var item = new { Name = "korte" };
        Assert.True((bool)m.Invoke(lookup, new object[] { item, "or" })!);
        Assert.False((bool)m.Invoke(lookup, new object[] { item, "zz" })!);
    }
}
