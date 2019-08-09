using System;
using System.IO;
using System.Runtime.CompilerServices;
using ListView_CollectionView_TableView.Droid;
using ListView_CollectionView_TableView.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PathService))]
namespace ListView_CollectionView_TableView.Droid
{
    public class PathService : IPathService
    {
        public string GetDataBasePath(string DatabaseName)
        {
            //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            //string path = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, DatabaseName);

            //ESTE FUNCIONA Y ME DEJA ACCEDER CON ADB PULL. ¿POR QUE?
            string path = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;


            return Path.Combine(path, DatabaseName);
        }
    }
}
