using System.Threading.Tasks;
using Wrecept.Core.Entities;

namespace Wrecept.Wpf.Services;

public record SetupData(string DatabasePath, string ConfigPath, UserInfo Info);

public interface ISetupFlow
{
    Task<SetupData> RunAsync(string defaultDb, string defaultCfg, IEnvironmentService? env = null);
}
