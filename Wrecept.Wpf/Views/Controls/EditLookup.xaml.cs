using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.Controls;

public partial class EditLookup : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(object), typeof(EditLookup));

    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
        nameof(SelectedValue), typeof(object), typeof(EditLookup),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
        nameof(SelectedValuePath), typeof(string), typeof(EditLookup));

    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath), typeof(string), typeof(EditLookup));

    public static readonly DependencyProperty CreateCommandProperty = DependencyProperty.Register(
        nameof(CreateCommand), typeof(ICommand), typeof(EditLookup));

    public static readonly DependencyProperty CreateCommandParameterProperty = DependencyProperty.Register(
        nameof(CreateCommandParameter), typeof(object), typeof(EditLookup));

    public object? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
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

    private TextBox? _textBox;

    public EditLookup()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _textBox = (TextBox)Box.Template.FindName("PART_EditableTextBox", Box);
        if (_textBox != null)
            _textBox.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (ItemsSource == null)
            return;
        var view = CollectionViewSource.GetDefaultView(ItemsSource);
        var text = _textBox?.Text ?? string.Empty;
        view.Filter = o => Matches(o, text);
        Box.IsDropDownOpen = true;
        view.Refresh();
    }

    private bool Matches(object? item, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return true;
        if (item is null)
            return false;
        var prop = item.GetType().GetProperty(DisplayMemberPath ?? "Name");
        var value = prop?.GetValue(item)?.ToString() ?? item.ToString();
        return value != null && value.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void Box_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (Box.SelectedItem == null)
            {
                var param = CreateCommandParameter ?? _textBox?.Text;
                if (CreateCommand?.CanExecute(param) == true)
                    CreateCommand.Execute(param);
            }
            else
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            Box.IsDropDownOpen = false;
            e.Handled = true;
        }
    }
}
