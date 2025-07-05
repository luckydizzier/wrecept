using System.Threading.Tasks;
using System.Threading;
namespace Wrecept.Core.Services;

public interface IBackupService
{
    Task BackupAsync(string destinationZipPath, CancellationToken ct = default);
    Task RestoreAsync(string zipPath, CancellationToken ct = default);
}
