using System;
using SQLite;

namespace ListView_CollectionView_TableView.Model
{
    public class DO_Users
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int Edad { get; set; }

        public string Genero { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
