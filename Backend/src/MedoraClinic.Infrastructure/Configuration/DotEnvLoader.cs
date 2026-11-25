using DotNetEnv;

namespace MedoraClinic.Infrastructure.Configuration;

public static class DotEnvLoader
{
    public static void Load(string? startingDirectory = null)
    {
        var start = new DirectoryInfo(
            startingDirectory ?? Directory.GetCurrentDirectory());

        while (start is not null)
        {
            var candidates = new[]
            {
                Path.Combine(start.FullName, ".env"),
                Path.Combine(start.FullName, "MedoraClinic.API", ".env"),
                Path.Combine(start.FullName, "Backend", "src", "MedoraClinic.API", ".env")
            };

            var envFile = candidates.FirstOrDefault(File.Exists);
            if (envFile is not null)
            {
                Env.Load(envFile, new LoadOptions(clobberExistingVars: false));
                return;
            }

            start = start.Parent;
        }

        Env.NoClobber().TraversePath().Load();
    }
}
