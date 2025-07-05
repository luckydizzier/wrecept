using Wrecept.Core.Services;

namespace Wrecept.Storage.Services;

public class SessionService : ISessionService
{
    private readonly string _path;

    public SessionService(string path)
    {
        _path = path;
    }

    public async Task<int?> LoadLastInvoiceIdAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_path))
            return null;
        var text = await File.ReadAllTextAsync(_path, ct);
        return int.TryParse(text, out var id) ? id : null;
    }

    public async Task SaveLastInvoiceIdAsync(int? invoiceId, CancellationToken ct = default)
    {
        if (invoiceId is null)
        {
            if (File.Exists(_path))
                File.Delete(_path);
            return;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        await File.WriteAllTextAsync(_path, invoiceId.Value.ToString(), ct);
    }
}
