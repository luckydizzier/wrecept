using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    private readonly IUserInfoService _service;
    private readonly string _baseText;

    [ObservableProperty]
    private string aboutText = string.Empty;

    public AboutViewModel(IUserInfoService service)
    {
        _service = service;

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Wrecept.Wpf.Resources.about_hu.md";
        using var stream = assembly.GetManifestResourceStream(resourceName);

        var text = "A Névjegy információ nem található.";
        if (stream is not null)
        {
            using var reader = new StreamReader(stream);
            text = StripFrontMatter(reader.ReadToEnd());
        }

        _baseText = text;
    }

    public async Task LoadAsync()
    {
        var info = await _service.LoadAsync();
        var sb = new StringBuilder(_baseText);

        if (!string.IsNullOrWhiteSpace(info.CompanyName))
        {
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine($"**Cégnév:** {info.CompanyName}");
            sb.AppendLine($"**Cím:** {info.Address}");
            sb.AppendLine($"**Telefon:** {info.Phone}");
            sb.AppendLine($"**E-mail:** {info.Email}");
        }

        AboutText = sb.ToString();
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
