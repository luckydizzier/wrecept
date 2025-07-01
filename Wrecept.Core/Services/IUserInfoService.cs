using Wrecept.Core.Entities;

namespace Wrecept.Core.Services;

public interface IUserInfoService
{
    Task<UserInfo> LoadAsync();
    Task SaveAsync(UserInfo info);
}
