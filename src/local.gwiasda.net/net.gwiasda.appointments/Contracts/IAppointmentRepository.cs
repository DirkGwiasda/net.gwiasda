namespace Net.Gwiasda.Appointments
{
    public interface IAppointmentRepository
    {
        Task CreateOrUpdateAppointmentAsync(Appointment booking);
        Task DeleteAppointmentIfExistsAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    }
}