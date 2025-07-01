using System.Text.Json;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Storage.Services;

public class UserInfoService : IUserInfoService
{
    private readonly string _path = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Wrecept", "wrecept.json");

    public async Task<UserInfo> LoadAsync()
    {
        if (!File.Exists(_path)) return new UserInfo();
        using var stream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<UserInfo>(stream) ?? new UserInfo();
    }

    public async Task SaveAsync(UserInfo info)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        using var stream = File.Create(_path);
        await JsonSerializer.SerializeAsync(stream, info, new JsonSerializerOptions { WriteIndented = true });
    }
}
