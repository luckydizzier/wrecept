using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.Controls;

public partial class SmartLookup : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(SmartLookup));

    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue), typeof(object), typeof(SmartLookup),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
        nameof(SelectedValuePath), typeof(string), typeof(SmartLookup));

    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath), typeof(string), typeof(SmartLookup));

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(SmartLookup),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
        nameof(Watermark), typeof(string), typeof(SmartLookup));

    public static readonly DependencyProperty CreateCommandProperty = DependencyProperty.Register(
        nameof(CreateCommand), typeof(ICommand), typeof(SmartLookup));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter), typeof(object), typeof(SmartLookup));

    public static readonly DependencyProperty MaxSuggestionsProperty = DependencyProperty.Register(
        nameof(MaxSuggestions), typeof(int), typeof(SmartLookup), new PropertyMetadata(10));

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

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Watermark
    {
        get => (string?)GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public ICommand? CreateCommand
    {
        get => (ICommand?)GetValue(CreateCommandProperty);
        set => SetValue(CreateCommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public int MaxSuggestions
    {
        get => (int)GetValue(MaxSuggestionsProperty);
        set => SetValue(MaxSuggestionsProperty, value);
    }

    private CancellationTokenSource? _cts;
    public ObservableCollection<object> FilteredItems { get; } = new();

    public SmartLookup()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (PART_TextBox != null)
        {
            PART_TextBox.GotFocus += TextBox_GotFocus;
            PART_TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (PART_TextBox != null)
        {
            PART_TextBox.GotFocus -= TextBox_GotFocus;
            PART_TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SmartLookup lookup)
            _ = lookup.FilterAsync();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        => _ = FilterAsync();

    private async Task FilterAsync()
    {
        var text = Text ?? string.Empty;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        var source = ItemsSource?.Cast<object>() ?? Enumerable.Empty<object>();
        var display = DisplayMemberPath;
        var max = MaxSuggestions;

        var results = await Task.Run(() =>
        {
            return source.Where(item => Match(item, text, display))
                         .Take(max)
                         .ToList();
        }, token);

        if (token.IsCancellationRequested) return;

        FilteredItems.Clear();
        foreach (var item in results)
            FilteredItems.Add(item);

        if (FilteredItems.Count == 0 && !string.IsNullOrWhiteSpace(text))
        {
            PART_CreatePrompt.Text = $"➕ Új létrehozása: '{text}'";
            PART_CreatePrompt.Visibility = Visibility.Visible;
        }
        else
        {
            PART_CreatePrompt.Text = string.Empty;
            PART_CreatePrompt.Visibility = Visibility.Collapsed;
        }

        PART_ListBox.SelectedIndex = FilteredItems.Count > 0 ? 0 : -1;
        PART_Popup.IsOpen = FilteredItems.Count > 0 || PART_CreatePrompt.Visibility == Visibility.Visible;
    }

    private static bool Match(object item, string text, string? display)
    {
        if (string.IsNullOrWhiteSpace(text))
            return true;
        var value = GetProperty(item, display)?.ToString();
        return value?.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static object? GetProperty(object item, string? path)
    {
        if (path is null)
            return item;
        var prop = item.GetType().GetProperty(path);
        return prop?.GetValue(item);
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down)
        {
            if (FilteredItems.Count > 0)
            {
                var index = PART_ListBox.SelectedIndex + 1;
                if (index >= FilteredItems.Count) index = 0;
                PART_ListBox.SelectedIndex = index;
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Up)
        {
            if (FilteredItems.Count > 0)
            {
                var index = PART_ListBox.SelectedIndex - 1;
                if (index < 0) index = FilteredItems.Count - 1;
                PART_ListBox.SelectedIndex = index;
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            if (PART_ListBox.SelectedItem != null)
            {
                SelectedValue = GetProperty(PART_ListBox.SelectedItem, SelectedValuePath);
                Text = GetProperty(PART_ListBox.SelectedItem, DisplayMemberPath)?.ToString() ?? string.Empty;
                PART_Popup.IsOpen = false;
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            else if (FilteredItems.Count == 0 && !string.IsNullOrWhiteSpace(Text))
            {
                var param = CommandParameter ?? Text;
                if (CreateCommand?.CanExecute(param) == true)
                    CreateCommand.Execute(param);
                PART_Popup.IsOpen = false;
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            PART_Popup.IsOpen = false;
            e.Handled = true;
        }
    }
}
