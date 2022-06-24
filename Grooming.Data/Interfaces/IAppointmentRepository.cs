using Grooming.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grooming.Data.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for appointment repository methods.
    /// </summary>
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();

        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);

        Task<Appointment> UpdateAppointmentByIdAsync(Appointment newAppointment, int appointmentId);

        Task<Appointment> CreateAppointmentAsync(Appointment appointment);

        Task<Appointment> DeleteAppointmentByIdAsync(int appointmentId);

    };
}
