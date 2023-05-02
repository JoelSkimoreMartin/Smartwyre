using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    private static IServiceProvider Services => _sp ??= StartUp.BuildServiceProvider();
    private static IServiceProvider _sp;

    public static async Task<int> Main(string[] args)
    {
        var commandLine = Services.GetService<CommandLine>();

        return await commandLine.InvokeAsync(args);
    }
}