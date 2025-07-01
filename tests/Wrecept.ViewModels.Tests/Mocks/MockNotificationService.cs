using System;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels.Tests.Mocks;

public class MockNotificationService : INotificationService
{
    public void ShowError(string message) => Console.WriteLine($"[MockError] {message}");
    public void ShowInfo(string message) => Console.WriteLine($"[MockInfo] {message}");
    public bool Confirm(string message) => true;
}
