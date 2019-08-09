using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ListView_CollectionView_TableView.Model;
using ListView_CollectionView_TableView.Services;
using SQLite;
using Xamarin.Forms;

namespace ListView_CollectionView_TableView
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        string DBPath = "";
        SQLiteAsyncConnection _sqlConection;

        public MainPage()
        {
            InitializeComponent();

            DBPath = DependencyService.Get<IPathService>().GetDataBasePath("MiBD.db");
            Debug.WriteLine("Ruta: " + DBPath);

            //DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite.db3");
            //Debug.WriteLine("Ruta2: " + DBPath);

            CreateTable();
        }

        private async Task CreateTable()
        {
            //Conexión a la Base de Datos
            _sqlConection = new SQLiteAsyncConnection(DBPath);

            await _sqlConection.CreateTableAsync<DO_Users>().ConfigureAwait(false);

            //DO_Users user = new DO_Users
            //{
            //    Nombre = "Jose A",
            //    Apellido = "Cedrón",
            //    Edad = 28,
            //    FechaNacimiento = DateTime.Now,
            //    Genero = "Masculino",
            //};

            //DO_Users user2 = new DO_Users
            //{
            //    Nombre = "Adela",
            //    Apellido = "Cedrón",
            //    Edad = 28,
            //    FechaNacimiento = DateTime.Now,
            //    Genero = "Masculino",
            //};

            //await SaveItemAsync(user);
            //await SaveItemAsync(user2);

            var users = await GetItemsAsync();

            foreach (DO_Users user in users)
            {
                Debug.WriteLine(user.Nombre);
            }
        }

        public Task<List<DO_Users>> GetItemsAsync()
        {
            return _sqlConection.Table<DO_Users>().ToListAsync();
        }

        public Task<List<DO_Users>> GetItemsNotDoneAsync()
        {
            return _sqlConection.QueryAsync<DO_Users>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<DO_Users> GetItemAsync(int id)
        {
            return _sqlConection.Table<DO_Users>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(DO_Users item)
        {
            if (item.ID != 0)
            {
                return _sqlConection.UpdateAsync(item);
            }
            else
            {
                return _sqlConection.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(DO_Users item)
        {
            return _sqlConection.DeleteAsync(item);
        }
    }
}