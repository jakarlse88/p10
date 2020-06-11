# Abarnathy

## Issues

This project is as of writing affected by the following issues:
- [mono/#19466](https://github.com/mono/mono/issues/19466): Mono's `msbuild` can't build Blazor WASM projects, therefore trying to run the project with `docker-compose` from Visual Studio Mac will fail (fix: run from the command line)
- [aspnetcore/#20613](github.com/dotnet/aspnetcore/issues/20613): Blazor WebAssembly ASP.NET Core hosted project does not copy the wwwroot to output, therefore the Blazor client will not work if the project is launched using `docker-compose` with the `Debug` configuration from Visual Studio (Windows). Fix: run using the commandline, or from VS using the `Release` configuration.