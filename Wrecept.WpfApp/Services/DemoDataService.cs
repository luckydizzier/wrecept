namespace Wrecept.WpfApp.Services;

public class DemoDataService : IDemoDataService
{
    public Task SeedAsync()
    {
        return Task.CompletedTask;
    }
}
