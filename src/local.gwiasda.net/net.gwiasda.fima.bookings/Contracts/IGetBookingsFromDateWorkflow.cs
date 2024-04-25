using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.FiMa
{
    public interface IGetBookingsFromDateWorkflow
    {
        Task<Dictionary<string, List<Booking>>> GetBookingsFromDay(DateTime day);
    }
}