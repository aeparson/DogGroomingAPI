using Grooming.Data.Model;
using Grooming.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grooming.Providers.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for appointment related service methods.
    /// </summary>
    public interface IAppointmentProvider
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();

        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);

        Task<Appointment> UpdateAppointmentByIdAsync(Appointment newAppointment, int appointmentId);

        Task<Appointment> CreateAppointmentAsync(Appointment newAppointment);

    }
}
