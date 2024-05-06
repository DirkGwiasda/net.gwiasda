namespace Net.Gwiasda.Appointments
{
    public interface IAppointmentValidator
    {
        Task Validate(Appointment appointment);
    }
}