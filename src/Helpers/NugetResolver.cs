using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dnlib.DotNet;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace Dotnetsafer.CLI.Helpers
{
    public class NugetResolver
    {
        private CancellationToken CancellationToken { get; }

        private PackageSearchResource LocalRepository { get; set; }

        public NugetResolver()
        {
            CancellationToken = CancellationToken.None;
        }

        private async Task InitializeAsync()
        {
            if (LocalRepository is not null)
                return; 

            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var rootPath = Path.Combine(userFolder, ".nuget", "packages");

            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3());

            var localSource = new PackageSource(rootPath);
            var localRepository = new SourceRepository(localSource, providers);

            LocalRepository = await localRepository
                .GetResourceAsync<PackageSearchResource>(CancellationToken);
        }

        public async Task<IEnumerable<string>> Resolve(string package, string version)
        {
            await InitializeAsync();

            var logger = NullLogger.Instance;

            var searchFilter = new SearchFilter(true)
            {
                IncludeDelisted = true,
            };

            var packages = await LocalRepository
                .SearchAsync(package, searchFilter, 0, 25, logger, CancellationToken.None);

            var directories = packages.Cast<PackageSearchMetadataBuilder.ClonedPackageSearchMetadata>()
                .Select(pk => Path.GetDirectoryName(pk.PackagePath));

            string[] searchPatterns = {"*.dll", "*.exe"};

            var validFiles = directories.SelectMany(dir => searchPatterns.SelectMany(pattern=>Directory.GetFiles(dir ?? string.Empty, pattern, SearchOption.AllDirectories))).ToList();

            bool IsExactVersion(string file)
            {
                try
                {
                    return ModuleDefMD.Load(file).Assembly.Version == new Version(version); 
                }
                catch (Exception)
                {
                    return false;
                }
             
            }

            bool IsSimilarVersion(string file)
            {
                try
                {
                    return new Version(version).Major == ModuleDefMD.Load(file).Assembly.Version.Major;
                }
                catch (Exception)
                {
                    return false;
                }

            }

            var exactAssemblies = validFiles.Where(IsExactVersion);

            var assemblies = exactAssemblies.ToList();

            return assemblies.Any() ? assemblies : validFiles.Where(IsSimilarVersion);
        }
    }
}
