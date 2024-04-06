using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace CustomGliders.PlayerContentLoaders
{
    public class PlayerContentLoader
    {
        public string ContentDirectoryPath;
        public string FileExtension;

        public PlayerContent<T>? LoadZIP<T>(string path, string expectedName)
        {
            using ZipArchive zipArchive = ZipFile.OpenRead(path);

            if (zipArchive.GetEntry("package.json") is not ZipArchiveEntry packageEntry)
            {
                Main.LogSource.LogWarning("Couldn't find package file in " + path);
                return null;
            }

            if (zipArchive.GetEntry(expectedName) is not ZipArchiveEntry resourceEntry)
            {
                Main.LogSource.LogWarning($"{expectedName} does not exist in " + path);
                return null;
            }

            Main.LogSource.LogDebug("Opening package");
            using Stream packageStream = packageEntry.Open();
            using StreamReader packageReader = new(packageStream);
            string packageJson = packageReader.ReadToEnd();
            
            T? package = UnityEngine.JsonUtility.FromJson<T>(packageJson);
            if (package is null)
            {
                Main.LogSource.LogWarning($"Failed to parse package as {typeof(T).FullName}");
                return null;
            }

            Main.LogSource.LogDebug("Reading resource file");
            using Stream resourceStream = resourceEntry.Open();
            using MemoryStream resourceMemStream = new();
            resourceStream.CopyTo(resourceMemStream);    

            return new PlayerContent<T>(package, resourceMemStream.ToArray());
        }

        public string[] GetFiles() => Directory.GetFiles(ContentDirectoryPath, "*." + FileExtension);

        public struct PlayerContent<T>
        {
            public T Package;
            public byte[] ContentResourceFile;

            public PlayerContent(T package, byte[] contentResourceFile)
            {
                Package = package;
                ContentResourceFile = contentResourceFile;
            }
        }
    }
}
