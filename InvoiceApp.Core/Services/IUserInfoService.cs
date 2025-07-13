using InvoiceApp.Core.Entities;

namespace InvoiceApp.Core.Services;

public interface IUserInfoService
{
    Task<UserInfo> LoadAsync();
    Task SaveAsync(UserInfo info);
}
