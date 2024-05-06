using Net.Gwiasda.Appointments;
using System.Text.Json;

namespace Net.Gwiasda.Local.Repository
{
    public class FileSystemAppointmentRepository : IAppointmentRepository
    {
        internal const string FileName = "appointments.dat";

        private readonly object _sync = new object();

        public Task CreateOrUpdateAppointmentAsync(Appointment appointment)
        {
            lock(_sync)
            {
                var appointments = GetAllAppointmentsAsync().Result.ToList();
                var existing = appointments.FirstOrDefault(l => l.Id == appointment.Id);
                if(existing != null)
                    appointments.Remove(existing);

                appointments.Add(appointment);

                SaveAppointments(appointments);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAppointmentIfExistsAsync(Guid id)
        {
            lock (_sync)
            {
                var appointments = GetAllAppointmentsAsync().Result.ToList();
                var existing = appointments.FirstOrDefault(l => l.Id == id);
                if (existing != null)
                {
                    appointments.Remove(existing);
                    SaveAppointments(appointments);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            var fileName = GetAppointmentsFileName();
            if(!File.Exists(fileName))
                return Enumerable.Empty<Appointment>();

            var json = await File.ReadAllTextAsync(fileName);
            if(string.IsNullOrWhiteSpace(json))
                return Enumerable.Empty<Appointment>();

            return JsonSerializer.Deserialize<IEnumerable<Appointment>>(json) ?? Enumerable.Empty<Appointment>();
        }

        private void SaveAppointments(IEnumerable<Appointment> appointments)
        {
            if (!Directory.Exists(FileSystemRepository.RootDataDirectory))
                Directory.CreateDirectory(FileSystemRepository.RootDataDirectory);

            var json = JsonSerializer.Serialize(appointments);
            File.WriteAllText(GetAppointmentsFileName(), json);
        }

        private string GetAppointmentsFileName() => Path.Combine(FileSystemRepository.RootDataDirectory, FileName);
    }
}