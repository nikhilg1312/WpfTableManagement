using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TableManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<ReservationDetails> reservedTables = MyStorage.ReadXML<ObservableCollection<ReservationDetails>>("reservedTables.xml");
        public static ObservableCollection<Table> tables = MyStorage.ReadXML<ObservableCollection<Table>>("tables.xml");
        public static bool IsMakeNewReservationOpen = false;
        public static bool IsEditReservationOpen = false;
        public static bool IsCVSnewReservationOpen = false;
        public static bool isDateTimeIsPast(DateTime ipDate,int time)
        {
            var whole_ipDate = new DateTime(ipDate.Year,ipDate.Month,ipDate.Day,Int32.Parse(time.ToString().Substring(0,2) ), Int32.Parse(time.ToString().Substring(2, 2)) ,0 );

            if (DateTime.Now > whole_ipDate)
                return true;
            return false;

        }

        public static bool isDateTimeIsPast(DateTime ipDate)
        {
            var whole_ipDate = new DateTime(ipDate.Year, ipDate.Month, ipDate.Day, 0, 0, 0);

            if (DateTime.Now.Date > whole_ipDate)
                return true;
            return false;

        }

        public static int getDataGridValueAt(DataGrid dGrid, int columnIndex)
        {
            if (dGrid.SelectedItem == null)
                return 0;
            string str = dGrid.SelectedItem.ToString(); // Take the selected line
            str = str.Replace("}", "").Trim().Replace("{", "").Trim(); // Delete useless characters
            if (columnIndex < 0 || columnIndex >= str.Split(',').Length) // case where the index can't be used 
                return 0;
            str = str.Split(',')[columnIndex].Trim();
            str = str.Split('=')[1].Trim();
            return Int32.Parse(str);
        }

    }
}
