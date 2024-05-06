using Moq;
using Net.Gwiasda.Appointments;

namespace net.gwiasda.appointments.tests
{
    public class HandleEndDatesTests
    {
        private Mock<IAppointmentManager> _appointmentManagerMock;
        private GetAppointmentsForTimespanWorkflow _test;

        private void Setup()
        {
            _appointmentManagerMock = new Mock<IAppointmentManager>();
            _test = new GetAppointmentsForTimespanWorkflow(_appointmentManagerMock.Object);
        }

        [Fact]
        public void Timespan_bothInMonth()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idTimespan = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = idTimespan, Date = new DateTime(2024, 3, 10), EndDate = new DateTime(2024, 3, 15), RecurringType = RecurringType.None },
                new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) }
            };

            // Act
            _test.HandleEndDates(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(3, appointments.Count);
            Assert.Contains(appointments, (a => a.Id == idDefault));
            Assert.Contains(appointments, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 3, 10).Date));
            Assert.Contains(appointments, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 3, 15).Date));
        }
        [Fact]
        public void Timespan_beforeUntilAfter()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idTimespan = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = idTimespan, Date = new DateTime(2024, 2, 10), EndDate = new DateTime(2024, 4, 15), RecurringType = RecurringType.None },
                new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) }
            };

            // Act
            _test.HandleEndDates(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(2, appointments.Count);
            Assert.Contains(appointments, (a => a.Id == idDefault));
            Assert.Contains(appointments, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 2, 10).Date));
        }
        [Fact]
        public void Timespan_startsIn()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idTimespan = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = idTimespan, Date = new DateTime(2024, 3, 10), EndDate = new DateTime(2024, 4, 15), RecurringType = RecurringType.None },
                new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) }
            };

            // Act
            _test.HandleEndDates(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(2, appointments.Count);
            Assert.Contains(appointments, (a => a.Id == idDefault));
            Assert.Contains(appointments, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 3, 10).Date));
        }
        [Fact]
        public void Timespan_endsIn()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idTimespan = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Id = idTimespan, Date = new DateTime(2024, 2, 10), EndDate = new DateTime(2024, 3, 15), RecurringType = RecurringType.None },
                new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) }
            };

            // Act
            _test.HandleEndDates(new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), appointments);

            // Assert
            Assert.Equal(3, appointments.Count);
            Assert.Contains(appointments, (a => a.Id == idDefault));
            Assert.Contains(appointments, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 2, 10).Date));
            Assert.Contains(appointments, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 3, 15).Date));
        }
    }
}