using CodeStats.Core.Analyze;
using Microsoft.Extensions.Hosting;

ICodeStatsAnalyzer analyzer = new CodeStatsCodeStatsAnalyzer();
await analyzer.Analyze(default);

using var host = Host.CreateDefaultBuilder(args).Build();
await host.StartAsync();

Environment.Exit(0);
