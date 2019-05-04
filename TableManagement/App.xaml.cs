using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TableManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<ReservationDetails> reservedTables = MyStorage.ReadXML<ObservableCollection<ReservationDetails>>("reservedTables.xml");
        public static ObservableCollection<Table> tables = MyStorage.ReadXML<ObservableCollection<Table>>("tables.xml");
    }
}
