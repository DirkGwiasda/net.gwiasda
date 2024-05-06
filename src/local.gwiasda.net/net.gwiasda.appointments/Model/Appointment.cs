namespace Net.Gwiasda.Appointments
{
    public class Appointment
    {
        public Guid Id { get; set; } = new Guid();
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string Text { get; set; } = string.Empty;
        public Person Who { get; set; } = Person.None;
        public RecurringType RecurringType { get; set; } = RecurringType.None;
        public string? GoogleMapsLink { get; set; }
        public DateTime? EndDate { get; set; }
        public bool NotInSchoolHolidays { get; set; } = false;
        public bool KeepAppointmentAfterItsEnd { get; set; } = false;
        public bool IsSchoolHoliday { get; set; } = false;

        public Appointment Clone()
        {
            return new Appointment
            {
                Id = this.Id,
                Title = this.Title,
                Date = this.Date,
                Text = this.Text,
                Who = this.Who,
                RecurringType = this.RecurringType,
                GoogleMapsLink = this.GoogleMapsLink,
                EndDate = this.EndDate,
                NotInSchoolHolidays = this.NotInSchoolHolidays,
                KeepAppointmentAfterItsEnd = this.KeepAppointmentAfterItsEnd,
                IsSchoolHoliday = this.IsSchoolHoliday
            };
        }
    }
}