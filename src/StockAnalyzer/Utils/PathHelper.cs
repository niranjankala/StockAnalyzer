using System;
using System.IO;
using System.Linq;

namespace StockAnalyzer.Utils
{
    public static class PathHelper
    {
        public static bool IsValidFilePath(string filePath)
        {
            bool result = true;
            try
            {
                result &= Path.IsPathRooted(filePath);
                result &= !string.IsNullOrWhiteSpace(Path.GetDirectoryName(filePath));
                result &= !string.IsNullOrWhiteSpace(Path.GetFileName(filePath));
            }
            catch (Exception)
            {
                return false;
            }
            return result;
        }

        public static string GetRootedPath(string fileName = "")
        {

            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }
            else
            {
#if FEATURE_APPDOMAIN
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
#else
                string baseDirectory = AppContext.BaseDirectory;
#endif
                return Path.Combine(baseDirectory, fileName);
            }

        }

    }
}
