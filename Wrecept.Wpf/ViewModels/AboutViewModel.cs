using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Reflection;

namespace Wrecept.Wpf.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    [ObservableProperty]
    private string aboutText = string.Empty;

    public AboutViewModel()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Wrecept.Wpf.Resources.about_hu.md";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is not null)
        {
            using var reader = new StreamReader(stream);
            AboutText = StripFrontMatter(reader.ReadToEnd());
        }
        else
        {
            AboutText = "A Névjegy információ nem található.";
        }
    }

    private static string StripFrontMatter(string text)
    {
        if (text.StartsWith("---"))
        {
            var end = text.IndexOf("---", 3);
            if (end != -1)
            {
                return text[(end + 3)..].Trim();
            }
        }
        return text.Trim();
    }
}
