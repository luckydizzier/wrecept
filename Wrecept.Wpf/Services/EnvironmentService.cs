using System;

namespace Wrecept.Wpf.Services;

public class EnvironmentService : IEnvironmentService
{
    public void Exit(int exitCode) => Environment.Exit(exitCode);
}
