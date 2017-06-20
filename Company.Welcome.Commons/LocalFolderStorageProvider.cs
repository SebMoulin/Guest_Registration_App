using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Company.Welcome.Commons
{
    public class LocalFolderStorageProvider : IProvideStorage
    {
        public async Task SaveFileAsync(string filename, string text, bool append)
        {
            var local = ApplicationData.Current.LocalFolder;
            StorageFile file;

            if (append)
            {
                file = await local.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            }
            else
            {
                file = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            using (var writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                writer.Write(text);
            };
        }

        public async Task<string> GetFileAsync(string filename)
        {
            var local = ApplicationData.Current.LocalFolder;
            if (local == null) return string.Empty;
            var file = await local.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            var text = await FileIO.ReadTextAsync(file);
            return text;
        }
    }
}