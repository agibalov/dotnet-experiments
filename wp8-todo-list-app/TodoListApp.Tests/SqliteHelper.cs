using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace TodoListApp.Tests
{
    public class SqliteHelper
    {
        public static async Task DropDatabaseIfExists(string dbName)
        {
            StorageFile file = null;
            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
            }
            catch (Exception)
            {
                // intentionally left blank
            }
            finally
            {
                if (file != null)
                {
                    await file.DeleteAsync();
                }
            }
        }

        public static async Task<string> CreateDatabaseFilePath(string dbName)
        {
            var dbFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(dbName);
            return dbFile.Path;
        }
    }
}
