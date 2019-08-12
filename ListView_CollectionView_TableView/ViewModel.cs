using System;
using System.Diagnostics;
using ListView_CollectionView_TableView.Helpers;
using ListView_CollectionView_TableView.Services;
using Xamarin.Forms;

namespace ListView_CollectionView_TableView
{
    public class ViewModel : Base_INotifedPropertyChanged
    {
        public Command CommandGenerarPDF { get; private set; }
        public Command CommandAbrirPDF { get; private set; }

        string path = "";

        public ViewModel()
        {
            CommandGenerarPDF = new Command(execute: async () => {
                _GenerarComPDFmand();
            });
            CommandAbrirPDF = new Command(_AbrirPDFCommand);

        }

        private void _AbrirPDFCommand()
        {
            var recib = DependencyService.Get<IOpenPDF>().OpenPDF(path);
        }

        private void _GenerarComPDFmand()
        {
            path = DependencyService.Get<IPathService>().GetDataBasePath("MiPDF.pdf");

            PDF_Tools.CreateNewPDF(path);
            Debug.WriteLine(path);
        }
    }
}
