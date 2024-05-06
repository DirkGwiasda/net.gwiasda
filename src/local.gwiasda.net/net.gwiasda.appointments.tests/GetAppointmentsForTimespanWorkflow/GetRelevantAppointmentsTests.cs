using Moq;
using Net.Gwiasda.Appointments;

namespace net.gwiasda.appointments.tests
{
    public class GetRelevantAppointmentsTests
    {
        private Mock<IAppointmentManager> _appointmentManagerMock;
        private GetAppointmentsForTimespanWorkflow _test;

        private void Setup()
        {
            _appointmentManagerMock = new Mock<IAppointmentManager>();
            _test = new GetAppointmentsForTimespanWorkflow(_appointmentManagerMock.Object);
        }

        [Fact]
        public void HappyPath()
        {
            // Arrange
            Setup();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var appointments = new List<Appointment>
            {
                new Appointment { Date = new DateTime(2024, 3, 10), RecurringType = RecurringType.Quarter },
                new Appointment { Date = new DateTime(2024, 3, 10), RecurringType = RecurringType.Yearly },
                new Appointment { Date = new DateTime(2024, 3, 10), RecurringType = RecurringType.BiWeekly },
                new Appointment { Id = id1, Date = new DateTime(2024, 3, 1) },
                new Appointment { Date = new DateTime(2024, 3, 10), RecurringType = RecurringType.Weekly },
                new Appointment { Id = id2, Date = new DateTime(2024, 3, 31) },
                new Appointment { Date = new DateTime(2024, 3, 10), RecurringType = RecurringType.Monthly }
            };

            // Act
            var result = _test.GetRelevantAppointments(appointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == id1));
            Assert.Contains(result, (a => a.Id == id1 && a.Date.Date == new DateTime(2024, 3, 1).Date));
            Assert.Contains(result, (a => a.Id == id2 && a.Date.Date == new DateTime(2024, 3, 31).Date));
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
            var result = _test.GetRelevantAppointments(appointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 3, 10).Date));
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
            var result = _test.GetRelevantAppointments(appointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 2, 10).Date));
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
            var result = _test.GetRelevantAppointments(appointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 3, 10).Date));
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
            var result = _test.GetRelevantAppointments(appointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idTimespan && a.Date.Date == new DateTime(2024, 2, 10).Date));
        }
    }
}