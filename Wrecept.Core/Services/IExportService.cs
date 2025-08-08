using System.Threading.Tasks;

namespace Wrecept.Core.Services;

public interface IExportService
{
    Task ExportAsync(string path);
    Task ImportAsync(string path);
}
