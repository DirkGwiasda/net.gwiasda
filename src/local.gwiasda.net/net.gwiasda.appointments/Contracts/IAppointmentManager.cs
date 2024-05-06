namespace Net.Gwiasda.Appointments
{
    public interface IAppointmentManager
    {
        Task CreateOrUpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentIfExistsAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
    }
}