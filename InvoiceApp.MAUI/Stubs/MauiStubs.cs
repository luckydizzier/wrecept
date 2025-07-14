using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.Controls
{
    public class VisualElement
    {
        public object? BindingContext { get; set; }
        public virtual void Focus() { }
    }

    public class Page : VisualElement
    {
        public INavigation Navigation { get; } = new NavigationStub();

        private class NavigationStub : INavigation
        {
            public System.Threading.Tasks.Task PushModalAsync(Page page) => System.Threading.Tasks.Task.CompletedTask;
            public System.Threading.Tasks.Task PopModalAsync() => System.Threading.Tasks.Task.CompletedTask;
        }

        public virtual System.Threading.Tasks.Task DisplayAlert(string title, string message, string cancel)
            => System.Threading.Tasks.Task.CompletedTask;
        public virtual System.Threading.Tasks.Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
            => System.Threading.Tasks.Task.FromResult(true);
    }

    public interface INavigation
    {
        System.Threading.Tasks.Task PushModalAsync(Page page);
        System.Threading.Tasks.Task PopModalAsync();
    }

    public class ContentPage : Page { }
    public class ContentView : VisualElement { }

    public class Entry : VisualElement
    {
        public string? Text { get; set; }
    }

    public class Application
    {
        public static Application? Current { get; set; }
        public Page? MainPage { get; set; }
    }

    public class Window : VisualElement
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class KeyEventArgs : System.EventArgs
    {
        public Key Key { get; }
        public bool IsDown { get; }
        public KeyEventArgs(Key key, bool isDown)
        {
            Key = key;
            IsDown = isDown;
        }
    }

    public enum Key
    {
        None,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        Up,
        Down,
        Insert,
        Delete,
        Enter,
        Return,
        Escape
    }
}

namespace Microsoft.Maui.ApplicationModel
{
    public static class MainThread
    {
        public static void BeginInvokeOnMainThread(System.Action action) => action();
    }
}
namespace Microsoft.Maui.Hosting
{
    public class MauiApp
    {
        public IServiceProvider Services { get; set; } = new ServiceCollection().BuildServiceProvider();
        public static MauiAppBuilder CreateBuilder() => new MauiAppBuilder();
    }


    public class MauiAppBuilder
    {
        public IServiceCollection Services { get; } = new ServiceCollection();
        public MauiAppBuilder UseMauiApp<T>() where T : class => this;
        public MauiApp Build() => new MauiApp { Services = Services.BuildServiceProvider() };
    }
}

namespace Microsoft.Maui
{
    public class MauiWinUIApplication
    {
        protected virtual Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => new Microsoft.Maui.Hosting.MauiApp();
        public void Run(string[] args) { }
    }
}

namespace Microsoft.Maui.Storage
{
    public static class FileSystem
    {
        public static string AppDataDirectory => "/tmp";
    }

    public class PickOptions
    {
        public string? PickerTitle { get; set; }
    }

    public class FilePickerResult
    {
        public string FullPath { get; set; } = string.Empty;
    }

    public class FilePicker
    {
        public static FilePicker Default { get; } = new FilePicker();
        public System.Threading.Tasks.Task<FilePickerResult?> PickAsync(PickOptions options) => System.Threading.Tasks.Task.FromResult<FilePickerResult?>(null);
    }
}
