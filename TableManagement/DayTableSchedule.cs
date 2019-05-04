using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagement
{
    class DayTableSchedule
    {
        public DateTime SelectedDate { get; set; }
        public int TableId { get; set; }
        public int[] TimeArray { get; set; }
    }
}
