using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditReservation.xaml
    /// </summary>
    public partial class EditReservation : Window
    {
        private ReservationDetails ipRDetails;

        public EditReservation(ReservationDetails _newReservation)
        {
            InitializeComponent();
            this.ipRDetails = _newReservation;
            this.Left = SystemParameters.WorkArea.Width - this.MaxWidth;
        }

        private void Btn_Reserve_Click(object sender, RoutedEventArgs e)
        {
            var selectedTableID = App.getDataGridValueAt(Dg_emptyTables, 0);


            // todo all ip checks
            if (!string.IsNullOrEmpty(TxtBx_rName.Text) & selectedTableID != 0 & (((Int32.Parse(Cbx_reservation_hours.Text) + 1) * 100) + Int32.Parse(Cbx_reservation_minute.Text) <=2300) )
            {
                App.reservedTables.Remove(ipRDetails);

                ipRDetails.NumberOfGuest = Int32.Parse(Cbx_guest_number.Text);
                ipRDetails.ReservationDate = (DateTime)Dtp_reservation_date.SelectedDate;
                ipRDetails.StartTime = Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text);
                ipRDetails.EndTime = ((Int32.Parse(Cbx_reservation_hours.Text) + 1) * 100) + Int32.Parse(Cbx_reservation_minute.Text);
                ipRDetails.GuestName = TxtBx_rName.Text;
                ipRDetails.TableId = selectedTableID;
                ipRDetails.IsActive =  1;

                //Console.WriteLine("IPdetails : " + ipRDetails.ReservationId + " |" + ipRDetails.TableId + " |" + ipRDetails.GuestName + " |" + ipRDetails.ReservationDate + " |" +ipRDetails.StartTime + " |" +ipRDetails.EndTime);
                


                if (MessageBox.Show("Are you sure with Reservation Details?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
            if (((Int32.Parse(Cbx_reservation_hours.Text) + 1) * 100) + Int32.Parse(Cbx_reservation_minute.Text) >2300)
            {
                MessageBox.Show("Please Select apropriate Time Slot", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.IsEditReservationOpen = true;

            Dtp_reservation_date.SelectedDate = ipRDetails.ReservationDate.Date;
            SelectByValue(Cbx_guest_number,ipRDetails.NumberOfGuest.ToString());
            SelectByValue(Cbx_reservation_hours,ipRDetails.StartTime.ToString().Substring(0,2));
            SelectByValue(Cbx_reservation_minute,ipRDetails.StartTime.ToString().Substring(2,2));
            TxtBx_rName.Text = ipRDetails.GuestName;
            Btn_Reserve.Visibility = Visibility.Collapsed;
            Dg_emptyTables.Visibility = Visibility.Collapsed;

            if (!App.isDateTimeIsPast(ipRDetails.ReservationDate.Date))
            {
                Btn_Load.Visibility = Visibility.Visible;
                Btn_Del.Visibility = Visibility.Visible;
            } 
            else
            {
                Btn_Load.Visibility = Visibility.Collapsed;
                Btn_Del.Visibility = Visibility.Visible;
            }
                


        }
        public static void SelectByValue(ComboBox cmb2, string value)
        {
            foreach (ComboBoxItem item in cmb2.Items)
                if (item.Content.ToString() == value)
                {
                    cmb2.SelectedValue = item;
                }
        }

        private void Dtp_reservation_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //CheckAvailiablityForNewParameters();
            if (Btn_Load != null & Btn_Reserve != null & Dg_emptyTables != null)
            {
                Btn_Load.Visibility = Visibility.Visible;
                Btn_Del.Visibility = Visibility.Visible;
                Btn_Reserve.Visibility = Visibility.Collapsed;
                Dg_emptyTables.Visibility = Visibility.Collapsed;
            }

        }

        private void Cbx_guest_number_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CheckAvailiablityForNewParameters();
            if (Btn_Load != null & Btn_Reserve != null & Dg_emptyTables != null)
            {
                Btn_Load.Visibility = Visibility.Visible;
                Btn_Del.Visibility = Visibility.Visible;
                Btn_Reserve.Visibility = Visibility.Collapsed;
                Dg_emptyTables.Visibility = Visibility.Collapsed;
            }
        }

        private void Cbx_reservation_hours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CheckAvailiablityForNewParameters();
            if (Btn_Load != null & Btn_Reserve != null & Dg_emptyTables != null)
            {
                Btn_Load.Visibility = Visibility.Visible;
                Btn_Del.Visibility = Visibility.Visible;
                Btn_Reserve.Visibility = Visibility.Collapsed;
                Dg_emptyTables.Visibility = Visibility.Collapsed;
            }
        }

        private void Cbx_reservation_minute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Btn_Load != null & Btn_Reserve != null & Dg_emptyTables != null)
            {
                Btn_Load.Visibility = Visibility.Visible;
                Btn_Del.Visibility = Visibility.Visible;
                Btn_Reserve.Visibility = Visibility.Collapsed;
                Dg_emptyTables.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtBx_rName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //CheckAvailiablityForNewParameters();
            if (Btn_Load != null & Btn_Reserve != null & Dg_emptyTables != null)
            {
                Btn_Load.Visibility = Visibility.Visible;
                Btn_Del.Visibility = Visibility.Visible;
                Btn_Reserve.Visibility = Visibility.Collapsed;
                Dg_emptyTables.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckAvailiablityForNewParameters()
        {


            if (Cbx_guest_number.Text.Equals(".") || Dtp_reservation_date == null || Cbx_reservation_hours.Text.Equals("Hrs")
                || Cbx_reservation_minute.Text.Equals("Min") || string.IsNullOrEmpty(TxtBx_rName.Text))
                Console.Write("First Run");
            
            else
            {
                //Console.WriteLine("Cbx_guest_number: " + Cbx_guest_number.Text);
                //Console.WriteLine("Dtp_reservation_date" + Dtp_reservation_date.SelectedDate.ToString());
                //Console.WriteLine("Hrs : Min " + Cbx_reservation_hours.Text + ":" + Cbx_reservation_minute.Text);
                //Console.WriteLine("Name :" + TxtBx_rName.Text);
                //Console.WriteLine("is past ? " + !App.isDateTimeIsPast((DateTime)Dtp_reservation_date.SelectedDate, Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text)));
                
                if (!Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate != null & !Cbx_reservation_hours.Text.Equals("Hrs")
                 & !Cbx_reservation_minute.Text.Equals("Min") & !string.IsNullOrEmpty(TxtBx_rName.Text)
                 & !App.isDateTimeIsPast((DateTime)Dtp_reservation_date.SelectedDate, Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text)))
                {
                    

                    App.reservedTables.Remove(ipRDetails);
                    int et_startTime = Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text);
                    int et_endtime = ((Int32.Parse(Cbx_reservation_hours.Text) + 1) * 100) + Int32.Parse(Cbx_reservation_minute.Text);

                    var booked_tables = (from et in App.reservedTables
                                         where
                                         et.ReservationDate.Date.Equals((DateTime)Dtp_reservation_date.SelectedDate) &&
                                         ((et_endtime > et.StartTime && et_endtime < et.EndTime) || (et_startTime > et.StartTime && et_startTime < et.EndTime)) &&
                                         et.IsActive ==1
                                         select et.TableId).ToList();
                    
                    var empty_tables = App.tables.Where(i => !booked_tables.Contains(i.TableId) && i.TableCapacity >= Int32.Parse(Cbx_guest_number.Text));

                    App.reservedTables.Add(ipRDetails);

                    if (empty_tables.Count() > 0)
                    {
                        Dg_emptyTables.Visibility = Visibility.Visible;
                        Dg_emptyTables.ItemsSource = (from et in empty_tables select new { et.TableId, et.TableCapacity }).ToList();
                    }
                    else
                    {
                        MessageBox.Show("No Tables Availiable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                    MessageBox.Show("Enter Appropriate Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        private void Btn_Load_Click(object sender, RoutedEventArgs e)
        {
            CheckAvailiablityForNewParameters();
            Btn_Reserve.Visibility = Visibility.Visible;
            Btn_Load.Visibility = Visibility.Collapsed;
            Btn_Del.Visibility = Visibility.Collapsed;

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
            App.IsEditReservationOpen = false;
            closingEtiquets();
        }

        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            App.reservedTables.Remove(ipRDetails);

            ipRDetails.IsActive = 0;

            Console.WriteLine("IPdetails : " + ipRDetails.ReservationId + " |" + ipRDetails.TableId + " |" + ipRDetails.GuestName + " |" + ipRDetails.ReservationDate + " |" +ipRDetails.StartTime + " |" +ipRDetails.EndTime + " |" + ipRDetails.IsActive);

            if (MessageBox.Show("Are you sure with Reservation Details?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.reservedTables.Add(ipRDetails);
                this.Close();
                closingEtiquets(ipRDetails.ReservationDate);
            }
        }
    }
}
