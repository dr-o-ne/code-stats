using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.CodeMetrics;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeStats.Core.Analyze;

public interface ICodeStatsAnalyzer
{
    Task Analyze(CancellationToken cancellationToken);
}

public sealed class CodeStatsCodeStatsAnalyzer : ICodeStatsAnalyzer
{
    private readonly string TargetPath = @"";

    public async Task Analyze(CancellationToken cancellationToken)
    {
        MSBuildLocator.RegisterDefaults();

        using var workspace = MSBuildWorkspace.Create();
        var solution = await workspace.OpenSolutionAsync(TargetPath, cancellationToken: cancellationToken);

        var totalSourceLines = 0L;
        var totalExecutableLines = 0L;

        foreach (var project in solution.Projects)
        {
            var projectCompilation = await project.GetCompilationAsync(cancellationToken);

            var projectMetrics = await CodeAnalysisMetricData.ComputeAsync(
                projectCompilation!.Assembly,
                new CodeMetricsAnalysisContext(projectCompilation, cancellationToken));

            totalSourceLines += projectMetrics.SourceLines;
            totalExecutableLines += projectMetrics.ExecutableLines;

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(project.AssemblyName);
            Console.WriteLine("Source Lines: " + projectMetrics.SourceLines);
            Console.WriteLine("Executable Lines: " + projectMetrics.ExecutableLines);
            Console.WriteLine("Maintainability Index: " + projectMetrics.MaintainabilityIndex);
            Console.WriteLine("Cyclomatic Complexity: " + projectMetrics.CyclomaticComplexity);
            Console.WriteLine("Depth of Inheritance: " + projectMetrics.DepthOfInheritance);
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("Total Source Lines (C#): " + totalSourceLines);
        Console.WriteLine("Total Executable Lines (C#): " + totalExecutableLines);
        Console.WriteLine(Environment.NewLine);
    }
}