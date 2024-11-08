#if NET6_0
using System;
using System.IO;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal
{
    internal static class AspNetCoreTempDirectory
    {
        private static string s_tempDirectory;

        public static string TempDirectory
        {
            get
            {
                if (s_tempDirectory == null)
                {
                    // Look for folders in the following order.
                    var temp = Environment.GetEnvironmentVariable("ASPNETCORE_TEMP") ?? // ASPNETCORE_TEMP - User set temporary location.
                               Path.GetTempPath();                                      // Fall back.

                    if (!Directory.Exists(temp))
                    {
                        throw new DirectoryNotFoundException(temp);
                    }

                    s_tempDirectory = temp;
                }

                return s_tempDirectory;
            }
        }

        public static Func<string> TempDirectoryFactory => () => TempDirectory;
    }
}
#endif
