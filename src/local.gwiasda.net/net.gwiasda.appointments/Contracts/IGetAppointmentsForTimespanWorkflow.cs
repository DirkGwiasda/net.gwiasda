namespace Net.Gwiasda.Appointments
{
    public interface IGetAppointmentsForTimespanWorkflow
    {
        Task<IEnumerable<Appointment>> GetAppointmentsForTimespanAsync(DateTime start, DateTime end);
    }
}