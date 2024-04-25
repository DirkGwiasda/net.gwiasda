using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Local.Repository
{
    public abstract class FiMaFileSystemRepository : FileSystemRepository
    {
        protected internal const string FiMaDirectory = "fima";

        protected internal string GetBaseDirectory(string concreteDirectoryName)
        {
            var dir = Path.Combine(RootDataDirectory, FiMaDirectory, concreteDirectoryName);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }
    }
}