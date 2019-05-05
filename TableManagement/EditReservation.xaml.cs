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
    /// Interaction logic for EditReservation.xaml
    /// </summary>
    public partial class EditReservation : Window
    {
        private ReservationDetails ipRDetails;
        private IEnumerable<Table> emptyTables;

        public EditReservation(ReservationDetails _newReservation)
        {
            InitializeComponent();
            this.ipRDetails = _newReservation;
        }

        private void Btn_Reserve_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
