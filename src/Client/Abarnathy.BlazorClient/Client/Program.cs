using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Abarnathy.BlazorClient.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            
            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
