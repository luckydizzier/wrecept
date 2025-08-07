using System.Threading.Tasks;

namespace Wrecept.Core.Services;

public interface IDemoDataService
{
    Task SeedAsync();
}
