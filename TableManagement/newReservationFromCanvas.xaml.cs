using System;
using System.Collections.Generic;
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
    /// Interaction logic for newReservationFromCanvas.xaml
    /// </summary>
    public partial class newReservationFromCanvas : Window
    {
        private ReservationDetails ipRDetails;

        public newReservationFromCanvas(ReservationDetails _newReservation)
        {
            InitializeComponent();
            this.ipRDetails = _newReservation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int u = ipRDetails.StartTime % 10;
            int t = (ipRDetails.StartTime / 10) % 10;
            int h = (ipRDetails.StartTime / 100) % 10;
            int th = ipRDetails.StartTime / 1000;

            TxtBlk_NoOfGuest.Text = ipRDetails.NumberOfGuest.ToString();
            TxtBlk_rDate.Text = ipRDetails.ReservationDate.Date.ToString("dd/MM/yyyy");
            TxtBlk_Time.Text = String.Concat(th.ToString(), h.ToString(), ":", t.ToString(), u.ToString());
            TxtBlk_TableID.Text = ipRDetails.TableId.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Btn_Reserve_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtBx_rName.Text))
                MessageBox.Show("Please enter Guest Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ipRDetails.GuestName = TxtBx_rName.Text;
                ipRDetails.ReservationId = App.reservedTables.Max(x => x.ReservationId) + 1;

                if (MessageBox.Show("Are you sure with Reservation Details?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    App.reservedTables.Add(ipRDetails);
                    this.Close();
                    closingEtiquets(ipRDetails.ReservationDate);
                }


            }
        }
        private void closingEtiquets(DateTime rDate)
        {
            ((MainWindow)this.Owner).TryTimeSlot(rDate);
            ((MainWindow)this.Owner).Dp_DatePicker.SelectedDate = rDate.Date;
        }


    }
}
