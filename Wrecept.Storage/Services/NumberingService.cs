using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Services;

namespace Wrecept.Storage.Services;

public class NumberingService : INumberingService
{
    private readonly string _path;
    private readonly SemaphoreSlim _mutex = new(1, 1);

    public NumberingService(string path)
    {
        _path = path;
    }

    public async Task<string> GetNextInvoiceNumberAsync(CancellationToken ct = default)
    {
        await _mutex.WaitAsync(ct);
        try
        {
            int current = 0;
            if (File.Exists(_path))
            {
                var text = await File.ReadAllTextAsync(_path, ct);
                if (int.TryParse(text, out var value))
                    current = value;
            }
            var next = current + 1;
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            await File.WriteAllTextAsync(_path, next.ToString(), ct);
            return $"INV{next}";
        }
        finally
        {
            _mutex.Release();
        }
    }
}
