using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TableManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ObservableCollection<ReservationDetails> reservedTables;
        //public ObservableCollection<Table> tables;
        DispatcherTimer timer_upcomings_15;

        public MainWindow()
        {
            InitializeComponent();
            timer_upcomings_15 = new DispatcherTimer();
            timer_upcomings_15.Interval = TimeSpan.FromSeconds(2);
            timer_upcomings_15.Tick += Timer_upcomings_15_Tick;
        }

        private void Timer_upcomings_15_Tick(object sender, EventArgs e)
        {
            LoadUpcomingFreeTables();
            LoadUpcomingGuest();
        }

        //private ObservableCollection<ReservationDetails> GenerateReservations()
        //{
        //    var lst = new ObservableCollection<ReservationDetails>();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        var sh = GetRandomHrs();
        //        var sm = GetRandomMin();
        //        lst.Add(new ReservationDetails { ReservationId = i, TableId = i ,  GuestName = "name" + i.ToString() , NumberOfGuest = 3,
        //                                         ReservationDate = DateTime.Today , StartHrs = sh , StartMin = sm });
        //    }
        //    return lst;
        //}

        //private int GetRandomHrs()
        //{
        //    int[] AllowedValues = new int[] { 17,18,19,20,21,22 };
        //    Random rand = new Random();
        //    return AllowedValues[rand.Next(AllowedValues.Length)];
        //}

        //private int GetRandomMin()
        //{
        //    int[] AllowedValues = new int[] { 00,15,30,45 };
        //    Random rand = new Random();
        //    return AllowedValues[rand.Next(AllowedValues.Length)];
        //}       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //reservedTables = MyStorage.ReadXML<ObservableCollection<ReservationDetails>>("reservedTables.xml");
            // = MyStorage.ReadXML<ObservableCollection<Table>>("tables.xml");
            //ObservableCollection<ReservationDetails>  reservedTables2 = GenerateReservations();
            //reservedTables = new ObservableCollection<ReservationDetails>(reservedTables1.Union(reservedTables2).ToList());
            //GetTableSchedule(reservedTables,DateTime.Today);
            timer_upcomings_15.Start();
            TryTimeSlot(DateTime.Today);

        }

        public void TryTimeSlot(DateTime ipDate)
        {
            Cvs_slot_1.Children.Clear();
            DrawVLines(Cvs_slot_1);
            DrawHLines(Cvs_slot_1);

            var lst = from r in App.reservedTables orderby r.TableId where r.ReservationDate.Date.Equals(ipDate.Date) select r;
            int[] tNum_all = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0 };
            //nice
            foreach (var item in lst)
            {
                int tNum = tNum_all[item.TableId];
                
                Rectangle bar = new Rectangle();

                var start_pxl = GetPxl(item.StartTime);
                var end_pxl = GetPxl(item.EndTime);

                bar.Width = end_pxl - start_pxl;
                bar.Height = (Cvs_slot_1.Height/9) - 2;

                var top_pxl = ((double)item.TableId -1 ) * (Cvs_slot_1.Height/9) ;
                
                bar.SetValue(Canvas.TopProperty, top_pxl);
                bar.SetValue(Canvas.LeftProperty, start_pxl);

                switch (tNum)
                {
                    case 0:
                        bar.Fill = Brushes.Gray;
                        tNum_all[item.TableId]++;
                        break;
                    case 1:
                        bar.Fill = Brushes.Green;
                        tNum_all[item.TableId]++;
                        break;
                    case 2:
                        bar.Fill = Brushes.LawnGreen;
                        tNum_all[item.TableId]++;
                        break;
                    case 4:
                        bar.Fill = Brushes.LightCyan;
                        tNum_all[item.TableId]++;
                        break;
                    case 5:
                        bar.Fill = Brushes.LightSeaGreen;
                        tNum_all[item.TableId]++;
                        break;
                }

                bar.ToolTip = item.GuestName;

                Cvs_slot_1.Children.Add(bar);    
            }


        }

        private void DrawHLines(Canvas cvs)
        {
            for (int i = 0; i < cvs.Height; i = i + (int)(cvs.Height/9) )
            {
                Line line = new Line();
                line.Stroke = Brushes.GreenYellow;

                line.X1 = 0;
                line.Y1 = i;
                line.X2 = cvs.Width;
                line.Y2 = i;

                line.StrokeThickness = 1;
                cvs.Children.Add(line);
            }
        }

        private void DrawVLines(Canvas cvs)
        {
            for (int i = 0; i < cvs.Width ; i = i + (int)(cvs.Width/6))
            {
                Line line = new Line();
                line.Stroke = Brushes.LightSteelBlue;

                line.X1 = i;
                line.Y1 = 0;
                line.X2 = i;
                line.Y2 = cvs.Height;

                line.StrokeThickness = 1;
                cvs.Children.Add(line);
            }
        }

        private double GetPxl(int ipPxl)
        {
            int u = ipPxl % 10;
            int t = (ipPxl / 10) % 10;
            int h = (ipPxl / 100) % 10;
            int th = ipPxl / 1000;

            var left_2 = th * 10 + h;
            var right_2 = t * 10 + u;

            return (double)(((left_2 - 17) * (Cvs_slot_1.Width/6)) + (right_2*2));

        }


        private void LoadUpcomingFreeTables()
        {
            var upg_lst = (from r in App.reservedTables
                           orderby r.EndTime
                           where
                           r.ReservationDate.Date == DateTime.Today.Date &&
                           r.EndTime > Int32.Parse(DateTime.Now.ToString("HHmm"))
                           select new { r.GuestName, r.TableId, r.EndTime }).ToList().Take(3);


            Dg_upcoming_Free_Tables.ItemsSource = upg_lst;
        }

        private void LoadUpcomingGuest()
        {
            var upg_lst = (from r in App.reservedTables
                           orderby r.StartTime
                           where
                           r.ReservationDate.Date == DateTime.Today.Date &&
                           r.StartTime > Int32.Parse(DateTime.Now.ToString("HHmm"))
                           select new { r.GuestName, r.TableId, r.StartTime }).ToList().Take(3);

            Dg_upcoming_Guest.ItemsSource = upg_lst;
                                                
        }

        private void GetTableSchedule(ObservableCollection<ReservationDetails> reservedTables, DateTime ipDate)
        {
            var lst = from r in reservedTables orderby r.StartTime where r.ReservationDate.Date.Equals(ipDate.Date) select r;
            dg_reservationOverview.ItemsSource = lst;
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            MyStorage.SaveXML<ObservableCollection<ReservationDetails>>(App.reservedTables, "reservedTables.xml");
        }


        private void Dp_DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetTableSchedule(App.reservedTables, (DateTime)Dp_DatePicker.SelectedDate);
            TryTimeSlot((DateTime)Dp_DatePicker.SelectedDate);
        }

        private void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            if ( !Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate != null & !Cbx_reservation_hours.Text.Equals("Hrs") 
                & !Cbx_reservation_minute.Text.Equals("Min") & string.IsNullOrEmpty(Tbx_reservation_name.Text) )
            {
                int et_startTime = Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text);
                int et_endtime = ((Int32.Parse(Cbx_reservation_hours.Text)+1) * 100) + Int32.Parse(Cbx_reservation_minute.Text);

                var booked_tables = (from et in App.reservedTables
                                      where
                                      et.ReservationDate.Date.Equals((DateTime)Dtp_reservation_date.SelectedDate) &&
                                      (( et_endtime > et.StartTime && et_endtime < et.EndTime ) || ( et_startTime > et.StartTime && et_startTime < et.EndTime ))
                                      select et.TableId).ToList();

                var empty_tables = App.tables.Where(i => !booked_tables.Contains(i.TableId) && i.TableCapacity >= Int32.Parse(Cbx_guest_number.Text));

                if (empty_tables.Count()> 0)
                {

                    ReservationDetails newReservation = new ReservationDetails
                    {
                        NumberOfGuest = Int32.Parse(Cbx_guest_number.Text),
                        ReservationDate = (DateTime)Dtp_reservation_date.SelectedDate,
                        StartTime = Int32.Parse(Cbx_reservation_hours.Text)*100 + Int32.Parse(Cbx_reservation_minute.Text),
                        EndTime = ((Int32.Parse(Cbx_reservation_hours.Text)+1) * 100) + Int32.Parse(Cbx_reservation_minute.Text),

                    };

                    var newReservationWindow = new makeNewReservation(newReservation, empty_tables);
                    newReservationWindow.Owner = this;
                    newReservationWindow.Show();
                }
                else
                {
                    MessageBox.Show("No Tables Availiable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            if (Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate != null & Cbx_reservation_hours.Text.Equals("Hrs")
                & Cbx_reservation_minute.Text.Equals("Min") & !string.IsNullOrEmpty(Tbx_reservation_name.Text))
            {
                var r_by_name_n_Date = (from r in App.reservedTables where r.GuestName.Equals(Tbx_reservation_name.Text) && r.ReservationDate.Date.Equals((DateTime)Dtp_reservation_date.SelectedDate) select r).FirstOrDefault();
                if (r_by_name_n_Date!=null)
                {
                    var editReservation = new EditReservation(r_by_name_n_Date);
                    editReservation.Show();
                }
                else
                    MessageBox.Show("No Reservation by Name " + Tbx_reservation_name.Text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate == null & Cbx_reservation_hours.Text.Equals("Hrs")
                & Cbx_reservation_minute.Text.Equals("Min") & !string.IsNullOrEmpty(Tbx_reservation_name.Text))
            {
                var r_by_name = (from r in App.reservedTables where r.GuestName.Equals(Tbx_reservation_name.Text)  select r).FirstOrDefault();
                if (r_by_name != null)
                {
                    var editReservation = new EditReservation(r_by_name);
                    editReservation.Show();
                }
                else
                    MessageBox.Show("No Reservation by Name " + Tbx_reservation_name.Text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }






        }
    }
}
