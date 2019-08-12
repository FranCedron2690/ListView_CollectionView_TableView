using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ListView_CollectionView_TableView
{
    public class Base_INotifedPropertyChanged : INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
