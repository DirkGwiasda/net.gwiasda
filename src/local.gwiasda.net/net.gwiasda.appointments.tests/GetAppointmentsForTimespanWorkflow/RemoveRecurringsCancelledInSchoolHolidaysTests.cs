using Moq;
using Net.Gwiasda.Appointments;

namespace net.gwiasda.appointments.tests
{
    public class RemoveRecurringsCancelledInSchoolHolidaysTests
    {
        private Mock<IAppointmentManager> _appointmentManagerMock;
        private GetAppointmentsForTimespanWorkflow _test;

        private void Setup()
        {
            _appointmentManagerMock = new Mock<IAppointmentManager>();
            _test = new GetAppointmentsForTimespanWorkflow(_appointmentManagerMock.Object);
        }

        [Fact]
        public void hitsOneDayHoliday()
        {
            // Arrange
            Setup();
            var id = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = id, Date = new DateTime(2024, 3, 12), RecurringType = RecurringType.Monthly, NotInSchoolHolidays = true },
                new Appointment { Date = new DateTime(2024, 3, 12), IsSchoolHoliday = true }
            };

            // Act
            _test.RemoveRecurringsCancelledInSchoolHolidays(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(1, appointments.Count);
            Assert.Contains(appointments, (a => a.Id != id));
        }
        [Fact]
        public void hitsHoliday()
        {
            // Arrange
            Setup();
            var id = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = id, Date = new DateTime(2024, 3, 1), RecurringType = RecurringType.Monthly, NotInSchoolHolidays = true },
                new Appointment { Date = new DateTime(2024, 2, 12), EndDate = new DateTime(2024, 3, 8), IsSchoolHoliday = true }
            };

            // Act
            _test.RemoveRecurringsCancelledInSchoolHolidays(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(1, appointments.Count);
            Assert.Contains(appointments, (a => a.Id != id));
        }
        [Fact]
        public void notHitsHoliday()
        {
            // Arrange
            Setup();
            var id = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = id, Date = new DateTime(2024, 3, 20), RecurringType = RecurringType.Monthly, NotInSchoolHolidays = true },
                new Appointment { Date = new DateTime(2024, 2, 12), EndDate = new DateTime(2024, 3, 8), IsSchoolHoliday = true }
            };

            // Act
            _test.RemoveRecurringsCancelledInSchoolHolidays(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(2, appointments.Count);
            Assert.Contains(appointments, (a => a.Id != id));
            Assert.Contains(appointments, (a => a.Id == id));
        }
        [Fact]
        public void holidayNotRelevant()
        {
            // Arrange
            Setup();
            var id = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = id, Date = new DateTime(2024, 3, 20), RecurringType = RecurringType.Yearly },
                new Appointment { Date = new DateTime(2024, 2, 20), EndDate = new DateTime(2024, 3, 22), IsSchoolHoliday = true }
            };

            // Act
            _test.RemoveRecurringsCancelledInSchoolHolidays(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(2, appointments.Count);
            Assert.Contains(appointments, (a => a.Id != id));
            Assert.Contains(appointments, (a => a.Id == id));
        }
    }
}