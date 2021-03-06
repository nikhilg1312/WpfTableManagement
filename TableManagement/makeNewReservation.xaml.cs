﻿using System;
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
            this.Left = SystemParameters.WorkArea.Width - this.MaxWidth;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.IsMakeNewReservationOpen = true;

            int u = ipRDetails.StartTime % 10;
            int t = (ipRDetails.StartTime / 10) % 10;
            int h = (ipRDetails.StartTime / 100) % 10;
            int th = ipRDetails.StartTime / 1000;

            TxtBlk_NoOfGuest.Text = ipRDetails.NumberOfGuest.ToString();
            TxtBlk_rDate.Text = ipRDetails.ReservationDate.Date.ToString("dd/MM/yyyy");
            TxtBlk_Time.Text = String.Concat( th.ToString(),h.ToString(),":",t.ToString(),u.ToString() );
            var toShowTables = (from et in emptyTables select new { et.TableId, et.TableCapacity }).ToList(); 
            Dg_emptyTables.ItemsSource = toShowTables;
            
        }

        private void Btn_Reserve_Click(object sender, RoutedEventArgs e)
        {
            var selectedTableID = App.getDataGridValueAt(Dg_emptyTables, 0);
            if (!string.IsNullOrEmpty(TxtBx_rName.Text) &  selectedTableID!= 0)
            {

                ipRDetails.ReservationId = App.reservedTables.Max(x => x.ReservationId) +1 ;
                ipRDetails.GuestName = TxtBx_rName.Text;
                ipRDetails.TableId = selectedTableID;

                //Console.WriteLine("IPdetails : " + ipRDetails.ReservationId + " |" + ipRDetails.TableId + " |" + ipRDetails.GuestName + " |" + ipRDetails.ReservationDate + " |" +ipRDetails.StartTime + " |" +ipRDetails.EndTime);

                if (MessageBox.Show("Are you sure with Reservation Details?","Warning",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    App.reservedTables.Add(ipRDetails);
                    this.Close();
                    closingEtiquets(ipRDetails.ReservationDate);
                }
            }
            if (!string.IsNullOrEmpty(TxtBx_rName.Text) & selectedTableID == 0)
            {
                MessageBox.Show("Please Select a Table", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (string.IsNullOrEmpty(TxtBx_rName.Text) & selectedTableID != 0)
            {
                MessageBox.Show("Please enter Guest Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void closingEtiquets(DateTime rDate)
        {
            ((MainWindow)this.Owner).TryTimeSlot(rDate);
            ((MainWindow)this.Owner).Dp_DatePicker.SelectedDate = rDate.Date;
            ((MainWindow)this.Owner).Dtp_reservation_date.SelectedDate = null;
            ((MainWindow)this.Owner).SelectByValueMain(((MainWindow)this.Owner).Cbx_guest_number, ".");
            ((MainWindow)this.Owner).SelectByValueMain(((MainWindow)this.Owner).Cbx_reservation_hours, "Hrs");
            ((MainWindow)this.Owner).SelectByValueMain(((MainWindow)this.Owner).Cbx_reservation_minute, "Min");
            ((MainWindow)this.Owner).Tbx_reservation_name.Text = null;
        }

        private void closingEtiquets()
        {
            ((MainWindow)this.Owner).Dtp_reservation_date.SelectedDate = null;
            ((MainWindow)this.Owner).SelectByValueMain(((MainWindow)this.Owner).Cbx_guest_number, ".");
            ((MainWindow)this.Owner).SelectByValueMain(((MainWindow)this.Owner).Cbx_reservation_hours, "Hrs");
            ((MainWindow)this.Owner).SelectByValueMain(((MainWindow)this.Owner).Cbx_reservation_minute, "Min");
            ((MainWindow)this.Owner).Tbx_reservation_name.Text = null;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.IsMakeNewReservationOpen = false;
            closingEtiquets();
        }
    }
}
