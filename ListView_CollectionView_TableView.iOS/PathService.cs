using System;
using System.IO;
using System.Runtime.CompilerServices;
using ListView_CollectionView_TableView.iOS;
using ListView_CollectionView_TableView.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PathService))]
namespace ListView_CollectionView_TableView.iOS
{
    public class PathService : IPathService
    {
        public string GetDataBasePath(string DatabaseName)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, DatabaseName);
        }
    }
}
