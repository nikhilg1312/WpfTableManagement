using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TableManagement
{
    /// <summary>
    /// Interaction logic for makeNewReservation.xaml
    /// </summary>
    public partial class makeNewReservation : Window
    {
        private ReservationDetails ipRDetails;
        private IEnumerable<Table> emptyTables;

        public makeNewReservation(ReservationDetails _newReservation, IEnumerable<Table> _empty_tables)
        {
            InitializeComponent();
            this.ipRDetails = _newReservation;
            this.emptyTables = _empty_tables;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int u = ipRDetails.StartTime % 10;
            int t = (ipRDetails.StartTime / 10) % 10;
            int h = (ipRDetails.StartTime / 100) % 10;
            int th = ipRDetails.StartTime / 1000;

            TxtBlk_NoOfGuest.Text = ipRDetails.NumberOfGuest.ToString();
            TxtBlk_rDate.Text = ipRDetails.ReservationDate.Date.ToString();
            TxtBlk_Time.Text = String.Concat( th.ToString(),h.ToString(),":",t.ToString(),u.ToString() );
            Dg_emptyTables.ItemsSource = emptyTables;
            
        }

        private void Btn_Reserve_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
