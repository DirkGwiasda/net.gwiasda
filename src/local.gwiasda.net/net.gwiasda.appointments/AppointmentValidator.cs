using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Appointments
{
    public class AppointmentValidator : IAppointmentValidator
    {
        public Task Validate(Appointment appointment)
        {
            if(appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            if(appointment.RecurringType != RecurringType.None && appointment.EndDate.HasValue)
                throw new ArgumentException("Recurring appointments cannot have a end-date.");

            return Task.CompletedTask;
        }
    }
}