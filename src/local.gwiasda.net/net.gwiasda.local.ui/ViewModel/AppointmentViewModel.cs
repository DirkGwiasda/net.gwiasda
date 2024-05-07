using Net.Gwiasda.Appointments;

namespace Net.Gwiasda.Local.UI.ViewModel
{
    public class AppointmentViewModel
    {
        private static readonly Dictionary<RecurringType, string> RecurringTypeMapping = new Dictionary<RecurringType, string>
        {
            { Net.Gwiasda.Appointments.RecurringType.None, "-" },
            { Net.Gwiasda.Appointments.RecurringType.Weekly, "wöchentlich" },
            { Net.Gwiasda.Appointments.RecurringType.BiWeekly, "alle 2 Wochen" },
            { Net.Gwiasda.Appointments.RecurringType.Monthly, "monatlich" },
            { Net.Gwiasda.Appointments.RecurringType.Quarter, "quartalsweise" },
            { Net.Gwiasda.Appointments.RecurringType.Yearly, "jährlich" }
        };

        public AppointmentViewModel() { }
        public AppointmentViewModel(Appointment appointment)
        {
            if (!RecurringTypeMapping.ContainsKey(appointment.RecurringType)) 
                throw new NotSupportedException($"RecurringType '{appointment.RecurringType}' is not supported.");

            var person = appointment.Who == Person.None ? "-" : Enum.GetName(typeof(Person), appointment.Who);

            Id = appointment.Id.ToString();
            Title = appointment.Title;
            Date = appointment.Date;
            Text = appointment.Text;
            Who = person;
            RecurringType = RecurringTypeMapping[appointment.RecurringType];
            GoogleMapsLink = appointment.GoogleMapsLink;
            EndDate = appointment.EndDate;
            NotInSchoolHolidays = appointment.NotInSchoolHolidays;
            KeepAppointmentAfterItsEnd = appointment.KeepAppointmentAfterItsEnd;
            IsSchoolHoliday = appointment.IsSchoolHoliday;
        }

        public string? Id { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string? Text { get; set; }
        public string? Who { get; set; }
        public string? RecurringType { get; set; }
        public string? GoogleMapsLink { get; set; }
        public DateTime? EndDate { get; set; }
        public bool NotInSchoolHolidays { get; set; } = false;
        public bool KeepAppointmentAfterItsEnd { get; set; } = false;
        public bool IsSchoolHoliday { get; set; } = false;

        public Appointment ToAppointment()
        {
            if (this.RecurringType == null || !RecurringTypeMapping.ContainsValue(this.RecurringType))
                throw new NotSupportedException($"RecurringType '{this.RecurringType}' is not supported.");

            return new Appointment
            {
                Id = Guid.Parse(Id),
                Title = Title ?? string.Empty,
                Date = Date,
                Text = Text,
                Who = GetPerson(),
                RecurringType = RecurringTypeMapping.FirstOrDefault(x => x.Value == RecurringType).Key,
                GoogleMapsLink = GoogleMapsLink,
                EndDate = EndDate,
                NotInSchoolHolidays = NotInSchoolHolidays,
                KeepAppointmentAfterItsEnd = KeepAppointmentAfterItsEnd,
                IsSchoolHoliday = IsSchoolHoliday
            };
        }
        private Person GetPerson()
        {
            if (string.IsNullOrWhiteSpace(Who) || Who == "-")
                return Person.None;

            if(!Enum.TryParse(Who, out Person person))
                throw new NotSupportedException($"Person '{Who}' is not supported.");

            return person;
        }
    }
}