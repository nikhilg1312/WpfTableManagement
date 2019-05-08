using Syncfusion.Windows.GridCommon;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TableManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            LoadCvsGui();
        }

        private void LoadCvsGui()
        {
            //status isActive
            var res = from r in App.reservedTables
                      orderby r.TableId
                      where r.ReservationDate.Date.Equals(DateTime.Today.Date) &
                            r.StartTime < Int32.Parse(DateTime.Now.ToString("HHmm")) &
                            r.EndTime > Int32.Parse(DateTime.Now.ToString("HHmm")) &
                            r.IsActive == 1
                      select new { r.TableId };

            int[] status_array = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 };

            foreach (var item in res)
            {
                status_array[item.TableId] = 1;
            }

            int qwe = 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    AddTable(i, j,qwe, status_array[qwe]);
                    qwe++;
                    
                }
            }

        }

        private void AddTable(int i, int j,int tableNum,int status)
        {
            var xPxl = (i+0.9)*80.0;
            var yPxl = (j+0.7)*80;

            var tableRec = new TextBlock
            {
                Height = 40,
                Width = 40,
                Text = string.Concat("T", tableNum.ToString())
            };

            if (status == 1)
                tableRec.Background = Brushes.Orange;
            else
                tableRec.Background = Brushes.GreenYellow;

            tableRec.SetValue(Canvas.TopProperty, xPxl);
            tableRec.SetValue(Canvas.LeftProperty, yPxl);
            Cvs_gui.Children.Add(tableRec);

        }

    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer_upcomings_15.Start();
            TryTimeSlot(DateTime.Today);
        }

        public void TryTimeSlot(DateTime ipDate)
        {
            Cvs_slot_1.Children.Clear();
            DrawVLines(Cvs_slot_1);
            DrawHLines(Cvs_slot_1);

            //status isActive
            var lst = from r in App.reservedTables orderby r.TableId where r.ReservationDate.Date.Equals(ipDate.Date) && r.IsActive ==1 select r;
            int[] tNum_all = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0 };

            foreach (var item in lst)
            {
                int tNum = tNum_all[item.TableId];
                
                var bar = new TextBlock();
                bar.Tag = item.ReservationId;
                bar.MouseUp += Bar_MouseUp;

                var start_pxl = GetPxl(item.StartTime);
                var end_pxl = GetPxl(item.EndTime);

                bar.Width = end_pxl - start_pxl;
                bar.Height = (Cvs_slot_1.Height/9) - 2;

                var top_pxl = ((double)item.TableId -1 ) * (Cvs_slot_1.Height/9) ;
                
                bar.SetValue(Canvas.TopProperty, top_pxl);
                bar.SetValue(Canvas.LeftProperty, start_pxl);

                switch (tNum)
                {
                    case 0://dfcce8
                        bar.Background = new BrushConverter().ConvertFromString("#dfcce8") as SolidColorBrush; ;
                        tNum_all[item.TableId]++;
                        break;
                    case 1:
                        bar.Background = new BrushConverter().ConvertFromString("#a4bed7") as SolidColorBrush; ;
                        tNum_all[item.TableId]++;
                        break;
                    case 2:
                        bar.Background = new BrushConverter().ConvertFromString("#c2d5b4") as SolidColorBrush; ;
                        tNum_all[item.TableId]++;
                        break;
                    case 4:
                        bar.Background = new BrushConverter().ConvertFromString("#cfc4a0") as SolidColorBrush; ;
                        tNum_all[item.TableId]++;
                        break;
                    case 5:
                        bar.Background = new BrushConverter().ConvertFromString("#c2c6c7") as SolidColorBrush; ;
                        tNum_all[item.TableId]++;
                        break;
                }

                bar.Text = item.GuestName;
                bar.TextAlignment = TextAlignment.Center;
                bar.VerticalAlignment = VerticalAlignment.Center;
                bar.HorizontalAlignment = HorizontalAlignment.Center;

                Cvs_slot_1.Children.Add(bar);    
            }


        }


        private void Bar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var rDate = DateTime.Now.Date;
            if (Dp_DatePicker.SelectedDate != null)
                rDate = (DateTime)Dp_DatePicker.SelectedDate;
            if (!App.isDateTimeIsPast(rDate))
            {

                var resID = (sender as TextBlock).Tag.ToString();

                //status isActive
                var r_by_name = (from r in App.reservedTables where r.ReservationId.Equals(Int32.Parse(resID)) && r.IsActive ==1 select r).FirstOrDefault();
                if (r_by_name != null & !App.IsEditReservationOpen)
                {
                    var editReservation = new EditReservation(r_by_name)
                    {
                        Owner = this
                    };
                    editReservation.Show();
                }

                
            }

            e.Handled = true;
        }

        private void DrawHLines(Canvas cvs)
        {
            for (int i = 0; i < cvs.Height; i = i + (int)(cvs.Height/9) )
            {
                Line line = new Line();
                line.Stroke = Brushes.DarkBlue;

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
            for (int i = 0; i < cvs.Width ; i = i + (int)(cvs.Width/24))
            {
                Line line = new Line();

                if (i % (cvs.Width / 6) == 0)
                    line.Stroke = Brushes.DarkBlue;
                else
                    line.Stroke = Brushes.LightBlue;

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
            //status isActive
            var upg_lst = (from r in App.reservedTables
                           orderby r.EndTime
                           where
                           r.ReservationDate.Date == DateTime.Today.Date &&
                           r.EndTime > Int32.Parse(DateTime.Now.ToString("HHmm")) &&
                           r.IsActive == 1
                           select new {
                                        Name = r.GuestName ,
                                        Table_Number = r.TableId,
                                        Time = string.Concat(r.EndTime.ToString().Substring(0, 2),":", r.EndTime.ToString().Substring(2, 2))
                                      }).ToList().Take(3);


            Dg_upcoming_Free_Tables.ItemsSource = upg_lst;
        }

        private void LoadUpcomingGuest()
        {
            //status isActive
            var upg_lst = (from r in App.reservedTables
                           orderby r.StartTime
                           where
                           r.ReservationDate.Date == DateTime.Today.Date &&
                           r.StartTime > Int32.Parse(DateTime.Now.ToString("HHmm")) &&
                           r.IsActive == 1
                           select new {
                                       Name = r.GuestName,
                                       Table_Number = r.TableId,
                                       Time = string.Concat(r.StartTime.ToString().Substring(0, 2), ":", r.StartTime.ToString().Substring(2, 2))
                           }).ToList().Take(3);

            Dg_upcoming_Guest.ItemsSource = upg_lst;
                                                
        }

        private void GetTableSchedule(ObservableCollection<ReservationDetails> reservedTables, DateTime ipDate)
        {
            //status isActive
            var lst = from r in reservedTables orderby r.StartTime where r.ReservationDate.Date.Equals(ipDate.Date) && r.IsActive == 1 select r;
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

            if (Dtp_reservation_date.SelectedDate == null)
            {
                if (  Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate == null & Cbx_reservation_hours.Text.Equals("Hrs")
                    & Cbx_reservation_minute.Text.Equals("Min") & !string.IsNullOrEmpty(Tbx_reservation_name.Text)                            )
                {
                    var r_by_name = (from r in App.reservedTables where r.GuestName.Equals(Tbx_reservation_name.Text) && r.IsActive == 1 select r).FirstOrDefault();
                    if (r_by_name != null & !App.IsEditReservationOpen)
                    {
                        var editReservation = new EditReservation(r_by_name)
                        {
                            Owner = this
                        };
                        editReservation.Show();
                    }
                    else
                        MessageBox.Show("No Reservation by Name " + Tbx_reservation_name.Text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (!Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate != null & !Cbx_reservation_hours.Text.Equals("Hrs")
                & !Cbx_reservation_minute.Text.Equals("Min") & string.IsNullOrEmpty(Tbx_reservation_name.Text) )
                {

                    if (!App.isDateTimeIsPast(ipDate: (DateTime)Dtp_reservation_date.SelectedDate, time: (int.Parse(Cbx_reservation_hours.Text) * 100) + int.Parse(Cbx_reservation_minute.Text)))
                    {

                        int et_startTime = Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text);
                        int et_endtime = ((Int32.Parse(Cbx_reservation_hours.Text) + 1) * 100) + Int32.Parse(Cbx_reservation_minute.Text);


                        if (et_endtime <= 2300)
                        {
                            var booked_tables = (from et in App.reservedTables
                                                 where
                                                 et.ReservationDate.Date.Equals((DateTime)Dtp_reservation_date.SelectedDate) &&
                                                 ((et_endtime > et.StartTime && et_endtime < et.EndTime) || (et_startTime > et.StartTime && et_startTime < et.EndTime)) &&
                                                 et.IsActive == 1
                                                 select et.TableId).ToList();
                            // todo check if time overlapps
                            var empty_tables = App.tables.Where(i => !booked_tables.Contains(i.TableId) && i.TableCapacity >= Int32.Parse(Cbx_guest_number.Text));

                            if (empty_tables.Count() > 0 & !App.IsMakeNewReservationOpen)
                            {

                                ReservationDetails newReservation = new ReservationDetails
                                {
                                    NumberOfGuest = Int32.Parse(Cbx_guest_number.Text),
                                    ReservationDate = (DateTime)Dtp_reservation_date.SelectedDate,
                                    StartTime = Int32.Parse(Cbx_reservation_hours.Text) * 100 + Int32.Parse(Cbx_reservation_minute.Text),
                                    EndTime = ((Int32.Parse(Cbx_reservation_hours.Text) + 1) * 100) + Int32.Parse(Cbx_reservation_minute.Text),
                                    IsActive = 1

                                };

                                var newReservationWindow = new makeNewReservation(newReservation, empty_tables)
                                {
                                    Owner = this
                                };
                                newReservationWindow.Show();
                            }
                        
                        }
                        else
                        {
                            MessageBox.Show("No Tables Availiable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }

                if (Cbx_guest_number.Text.Equals(".") & Dtp_reservation_date.SelectedDate != null & Cbx_reservation_hours.Text.Equals("Hrs")
                    & Cbx_reservation_minute.Text.Equals("Min") & !string.IsNullOrEmpty(Tbx_reservation_name.Text))
                {
                    var r_by_name_n_Date = (from r in App.reservedTables
                                            where r.GuestName.Equals(Tbx_reservation_name.Text) &&
                                            r.ReservationDate.Date.Equals((DateTime)Dtp_reservation_date.SelectedDate) &&
                                            r.IsActive == 1
                                            select r).FirstOrDefault();
                    if (r_by_name_n_Date != null & !App.IsEditReservationOpen)
                    {
                        var editReservation = new EditReservation(r_by_name_n_Date)
                        {
                            Owner = this
                        };
                        editReservation.Show();
                    }
                    else
                        MessageBox.Show("No Reservation by Name " + Tbx_reservation_name.Text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        public void SelectByValueMain(ComboBox cmb2, string value)
        {
            foreach (ComboBoxItem item in cmb2.Items)
                if (item.Content.ToString() == value)
                {
                    cmb2.SelectedValue = item;
                }
        }

        private int getStartTimeGUI(double x)
        {

            var cell = Math.Ceiling((x) / (Cvs_slot_1.Width / 24));
            var HH = 0.0;
            if (cell % 4 == 0)
                HH = (Math.Floor((cell - 1) / 4) + 17) * 100;
            else
                HH = (Math.Floor(cell / 4) + 17) * 100;

            int mm = Convert.ToInt32(((cell - 1) % 4) * 15);
            int stime = Convert.ToInt32(HH + mm);

            return stime;
        }

        private void Cvs_slot_1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var rDate = DateTime.Now.Date;
            if (Dp_DatePicker.SelectedDate != null)
                rDate = (DateTime)Dp_DatePicker.SelectedDate;

            Console.WriteLine("Date : " + rDate);

            if (!App.isDateTimeIsPast(rDate))
            {
                var pos = e.GetPosition(Cvs_slot_1);

                var TableID = Math.Ceiling(pos.Y / (Cvs_slot_1.Height / 9));
                int startTime = getStartTimeGUI(pos.X);
                int endTime = startTime + 100;


                Console.WriteLine("start Time " + startTime);
                Console.WriteLine("end Time " + endTime);
                

                var booked_tables = (from et in App.reservedTables
                                     where
                                     et.ReservationDate.Date.Equals(rDate) &&
                                     et.TableId.Equals(Convert.ToInt32(TableID)) &&
                                     ((endTime > et.StartTime && endTime < et.EndTime) || (startTime > et.StartTime && startTime < et.EndTime)) &&
                                     et.IsActive == 1
                                     select et.TableId).ToList();

                Console.WriteLine("IsCVSnewReservationOpen : " +App.IsCVSnewReservationOpen);

                if (booked_tables.Count() == 0 & endTime <= 2300 & !App.IsCVSnewReservationOpen)
                {

                    ReservationDetails reservationDetails = new ReservationDetails
                    {
                        StartTime = startTime,
                        EndTime = startTime + 100,
                        TableId = Convert.ToInt32(TableID),
                        ReservationDate = rDate,
                        NumberOfGuest = (from tbx in App.tables where tbx.TableId.Equals(Convert.ToInt32(TableID)) select tbx.TableCapacity).FirstOrDefault(),
                        IsActive = 1
                        
                    };


 
                        var newCvsReservation = new newReservationFromCanvas(reservationDetails)
                        {
                            Owner = this
                        };
                        newCvsReservation.Show();

                }
                else
                    MessageBox.Show("Time Overlapped! Please select other time slot", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

           


        }


    }
}
