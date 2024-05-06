using Moq;
using Net.Gwiasda.Appointments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.gwiasda.appointments.tests
{
    public class HandleRecurringAppointmentsTests
    {
        private Mock<IAppointmentManager> _appointmentManagerMock;
        private GetAppointmentsForTimespanWorkflow _test;

        private void Setup()
        {
            _appointmentManagerMock = new Mock<IAppointmentManager>();
            _test = new GetAppointmentsForTimespanWorkflow(_appointmentManagerMock.Object);
        }

        [Fact]
        public void notInTimespan()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();

            var simple = new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) };
            var allAppointments = new List<Appointment>
            {
                new Appointment { Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Quarter },
                simple
            };
            var result = new List<Appointment>
            {
                simple
            };

            // Act
            _test.HandleRecurringAppointments(allAppointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), result);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, (a => a.Id == idDefault));
        }
        [Fact]
        public void monthly()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idRecurring = Guid.NewGuid();

            var simple = new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) };
            var allAppointments = new List<Appointment>
            {
                new Appointment { Id = idRecurring, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Monthly },
                new Appointment { Id = Guid.Empty, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Quarter },
                new Appointment { Id = Guid.Empty, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Yearly },
                simple
            };
            var result = new List<Appointment>
            {
                simple
            };

            // Act
            _test.HandleRecurringAppointments(allAppointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), result);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 10).Date));
        }
        [Fact]
        public void quarter()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idRecurring = Guid.NewGuid();

            var simple = new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) };
            var allAppointments = new List<Appointment>
            {
                new Appointment { Id = idRecurring, Date = new DateTime(2023, 12, 10), RecurringType = RecurringType.Quarter },
                new Appointment { Id = Guid.Empty, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Yearly },
                simple
            };
            var result = new List<Appointment>
            {
                simple
            };

            // Act
            _test.HandleRecurringAppointments(allAppointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), result);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 10).Date));
        }
        [Fact]
        public void yearly()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idRecurring = Guid.NewGuid();

            var simple = new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) };
            var allAppointments = new List<Appointment>
            {
                new Appointment { Id = idRecurring, Date = new DateTime(2023, 3, 12), RecurringType = RecurringType.Yearly },
                new Appointment { Id = Guid.Empty, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Yearly },
                simple
            };
            var result = new List<Appointment>
            {
                simple
            };

            // Act
            _test.HandleRecurringAppointments(allAppointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), result);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 12).Date));
        }
        [Fact]
        public void weekly()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idRecurring = Guid.NewGuid();

            var simple = new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) };
            var allAppointments = new List<Appointment>
            {
                new Appointment { Id = idRecurring, Date = new DateTime(2023, 10, 2), RecurringType = RecurringType.Weekly },
                new Appointment { Id = Guid.Empty, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Yearly },
                simple
            };
            var result = new List<Appointment>
            {
                simple
            };

            // Act
            _test.HandleRecurringAppointments(allAppointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), result);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 4).Date));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 11).Date));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 18).Date));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 25).Date));
        }
        [Fact]
        public void biweekly()
        {
            // Arrange
            Setup();
            var idDefault = Guid.NewGuid();
            var idRecurring = Guid.NewGuid();

            var simple = new Appointment { Id = idDefault, Date = new DateTime(2024, 3, 12) };
            var allAppointments = new List<Appointment>
            {
                new Appointment { Id = idRecurring, Date = new DateTime(2023, 10, 2), RecurringType = RecurringType.BiWeekly },
                new Appointment { Id = Guid.Empty, Date = new DateTime(2024, 1, 10), RecurringType = RecurringType.Yearly },
                simple
            };
            var result = new List<Appointment>
            {
                simple
            };

            // Act
            _test.HandleRecurringAppointments(allAppointments, new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), result);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, (a => a.Id == idDefault));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 4).Date));
            Assert.Contains(result, (a => a.Id == idRecurring && a.Date.Date == new DateTime(2024, 3, 18).Date));
        }
    }
}