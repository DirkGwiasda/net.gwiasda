using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public class RecurringBooking : Booking
    {
        public RecurringType RecurringType { get; set; }
        public DateTime? EndDate { get; set; }
    }
}