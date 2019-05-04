using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagement
{
    public class ReservationDetails
    {
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        public string GuestName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int NumberOfGuest { get; set; }
    }

}
