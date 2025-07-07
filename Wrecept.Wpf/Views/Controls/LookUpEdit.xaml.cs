using System;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf.Views.Controls;

public partial class LookUpEdit : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(LookUpEdit));

    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue), typeof(object), typeof(LookUpEdit),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
        nameof(SelectedValuePath), typeof(string), typeof(LookUpEdit));

    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath), typeof(string), typeof(LookUpEdit));

    public static readonly DependencyProperty CreateCommandProperty = DependencyProperty.Register(
        nameof(CreateCommand), typeof(ICommand), typeof(LookUpEdit));

    public static readonly DependencyProperty CreateCommandParameterProperty = DependencyProperty.Register(
        nameof(CreateCommandParameter), typeof(object), typeof(LookUpEdit));

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);

        set => SetValue(ItemsSourceProperty, value);
    }

    public object? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public string? SelectedValuePath
    {
        get => (string?)GetValue(SelectedValuePathProperty);
        set => SetValue(SelectedValuePathProperty, value);
    }

    public string? DisplayMemberPath
    {
        get => (string?)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    public object? CreateCommandParameter
    {
        get => GetValue(CreateCommandParameterProperty);
        set => SetValue(CreateCommandParameterProperty, value);
    }

    public ICommand? CreateCommand
    {
        get => (ICommand?)GetValue(CreateCommandProperty);
        set => SetValue(CreateCommandProperty, value);
    }

    public LookUpEdit()
    {
        InitializeComponent();
    }

}
