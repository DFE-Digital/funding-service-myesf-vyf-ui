Instructions for adding StyleCop when adding new projects to the solution:

1. Add the StyleCop.Analyzers nuget package
2. Add stylecop.json from this Shared project as a linked file in the root of your new project
3. In the properties of the linked stylecop.json file, change the build action to "c# analyzer additional file"
4. Update the csproj file for your new project to ensure it contains the following:

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <CodeAnalysisRuleSet>..\$safeprojectname$\PDS.CodeAnalysis.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <CodeAnalysisRuleSet>..\$safeprojectname$\PDS.CodeAnalysis.ruleset</CodeAnalysisRuleSet>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>