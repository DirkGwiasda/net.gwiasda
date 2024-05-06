namespace Net.Gwiasda.Appointments
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly IAppointmentValidator _validator;
        private readonly IAppointmentRepository _repository;

        public AppointmentManager(IAppointmentValidator validator, IAppointmentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task CreateOrUpdateAppointmentAsync(Appointment appointment)
        {
            await _validator.Validate(appointment);
            await _repository.CreateOrUpdateAppointmentAsync(appointment);
        }

        public async Task DeleteAppointmentIfExistsAsync(Guid id)
            => await _repository.DeleteAppointmentIfExistsAsync(id);

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
            => await _repository.GetAllAppointmentsAsync();
        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            var allApointsments = await GetAllAppointmentsAsync();
            var result = allApointsments.FirstOrDefault(a => a.Id == id);
            if (result == null)
                throw new InvalidOperationException($"Appointment with id '{id}' not found!");

            return result;
        }
    }
}