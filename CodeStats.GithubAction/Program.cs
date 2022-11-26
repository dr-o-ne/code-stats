using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args).Build();
await host.StartAsync();

Console.WriteLine("CodeStats Hello World!");
Environment.Exit(0);
