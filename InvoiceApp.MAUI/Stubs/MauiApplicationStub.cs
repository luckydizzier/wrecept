namespace Microsoft.Maui;

public abstract class MauiApplication
{
    protected MauiApplication() { }

    protected abstract MauiApp CreateMauiApp();

    public void Run(string[] args) { }
}

