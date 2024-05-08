namespace Net.Gwiasda.Appointments
{
    public class GetAppointmentsForTimespanWorkflow : IGetAppointmentsForTimespanWorkflow
    {
        private readonly IAppointmentManager _appointmentManager;

        public GetAppointmentsForTimespanWorkflow(IAppointmentManager appointmentManager)
        {
            _appointmentManager = appointmentManager ?? throw new ArgumentNullException(nameof(appointmentManager));
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForTimespanAsync(DateTime start, DateTime end)
        {
            var appointments = (await _appointmentManager.GetAllAppointmentsAsync()).ToList();
            await DeleteOldAppointments(appointments);

            var result = GetRelevantAppointments(appointments, start, end);

            HandleEndDates(start, end, result);
            HandleRecurringAppointments(appointments, start, end, result);
            RemoveRecurringsCancelledInSchoolHolidays(start, end, result);

            result.Sort((a, b) => a.Date.CompareTo(b.Date));

            return result;
        }

        internal List<Appointment> GetRelevantAppointments(IEnumerable<Appointment> appointments, DateTime start, DateTime end)
        => appointments.Where(a =>
                a.RecurringType == RecurringType.None
                && ((a.Date >= start && a.Date <= end) ||
                    (a.Date < end && a.EndDate.HasValue && a.EndDate.Value > start))
            ).ToList();
        internal void HandleEndDates(DateTime start, DateTime end, List<Appointment> result)
        {
            var itemsToAdd = new List<Appointment>();
            foreach(var appointment in result.Where(a => a.EndDate.HasValue))
            {
                if (appointment.EndDate.Value <= end)
                {
                    var endAppointment = appointment.Clone();
                    endAppointment.Date = appointment.EndDate.Value;
                    endAppointment.Title = $"Ende: {endAppointment.Title}";
                    itemsToAdd.Add(endAppointment);
                }
            }
            result.AddRange(itemsToAdd);
        }
        internal void HandleRecurringAppointments(IEnumerable<Appointment> appointments, DateTime start, DateTime end, List<Appointment> result)
        {
            var recurringAppointments = appointments.Where(a => a.RecurringType != RecurringType.None).ToList();

            foreach (var recurringAppointment in recurringAppointments)
            {
                var current = recurringAppointment.Date;
                while (current.Date <= end.Date)
                {
                    if (current.Date >= start.Date)
                    {
                        var clone = recurringAppointment.Clone();
                        clone.Date = current;
                        result.Add(clone);
                    }

                    switch (recurringAppointment.RecurringType)
                    {
                        case RecurringType.Weekly:
                            current = current.AddDays(7);
                            break;
                        case RecurringType.BiWeekly:
                            current = current.AddDays(14);
                            break;
                        case RecurringType.Monthly:
                            current = current.AddMonths(1);
                            break;
                        case RecurringType.Quarter:
                            current = current.AddMonths(3);
                            break;
                        case RecurringType.Yearly:
                            current = current.AddYears(1);
                            break;
                        default:
                            throw new NotSupportedException($"RecurringType '{recurringAppointment.RecurringType}' is not supported.");
                    }
                }
            }
        }
        internal void RemoveRecurringsCancelledInSchoolHolidays(DateTime start, DateTime end, List<Appointment> result)
        {
            var holidays = result.Where(a => a.IsSchoolHoliday).ToList();
            var recurrings = result.Where(a => a.RecurringType != RecurringType.None && a.NotInSchoolHolidays).ToList();
            foreach(var holiday in holidays)
            {
                var endDate = holiday.EndDate ?? holiday.Date;
                var removeMe = recurrings.Where(r => r.Date.Date >= holiday.Date.Date && r.Date.Date <= endDate.Date).ToList();
                foreach(var remove in removeMe)
                    result.Remove(remove);
            }
        }
        internal async Task DeleteOldAppointments(List<Appointment> appointments)
        {
            var removeMe = new List<Appointment>();
            foreach(var appointment in appointments)
            {
                var endDate = appointment.EndDate ?? appointment.Date;
                if (DateTime.Now - endDate > TimeSpan.FromDays(31))
                {
                    await _appointmentManager.DeleteAppointmentIfExistsAsync(appointment.Id);
                    removeMe.Add(appointment);
                }
            }
            foreach(var appointment in removeMe)
                appointments.Remove(appointment);
        }
    }
}