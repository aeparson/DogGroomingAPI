using Grooming.Data.Context;
using Grooming.Data.Interfaces;
using Grooming.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grooming.Data.Filters;

namespace Grooming.Data.Repositories
{
    /// <summary>
    /// This class handles methods for making requests to the appointment repository.
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ILogger<AppointmentRepository> _logger;
        private readonly IPetCtx _ctx;

        public AppointmentRepository(ILogger<AppointmentRepository> logger, IPetCtx ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _ctx.Appointments
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _ctx.Appointments
                .AsNoTracking()
                .WhereAppointmentIdEquals(appointmentId)
                .SingleOrDefaultAsync();
        }


        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            await _ctx.Appointments.AddAsync(appointment);
            await _ctx.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment> DeleteAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _ctx.Appointments.AsNoTracking().WhereAppointmentIdEquals(appointmentId).SingleOrDefaultAsync();
            _ctx.Appointments.Remove(appointment);
            await _ctx.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> UpdateAppointmentByIdAsync(Appointment newAppointment, int appointmentId)
        {
            var appointment = await _ctx.Appointments.AsNoTracking().WhereAppointmentIdEquals(appointmentId).SingleOrDefaultAsync();

            appointment.Id = appointment.Id;

            if (newAppointment.PetId != 0)
            {
                appointment.PetId = newAppointment.PetId;
            }
            else
            {
                appointment.PetId = appointment.PetId;
            }

            if (newAppointment.Shampoos != null)
            {
                appointment.Shampoos = newAppointment.Shampoos;
            }
            else
            {
                appointment.Shampoos = appointment.Shampoos;
            }

            if (newAppointment.Conditioners != null)
            {
                appointment.Conditioners = newAppointment.Conditioners;
            }
            else
            {
                appointment.Conditioners = appointment.Conditioners;
            }

            if (newAppointment.Groomer != null)
            {
                appointment.Groomer = newAppointment.Groomer;
            }
            else
            {
                appointment.Groomer = appointment.Groomer;
            }

            if (newAppointment.BathMethods != null)
            {
                appointment.BathMethods = newAppointment.BathMethods;
            }
            else
            {
                appointment.BathMethods = appointment.BathMethods;
            }

            if (newAppointment.BrushInfo != null)
            {
                appointment.BrushInfo = newAppointment.BrushInfo;
            }
            else
            {
                appointment.BrushInfo = appointment.BrushInfo;
            }

            if (newAppointment.BehaviorNotes != null)
            {
                appointment.BehaviorNotes = newAppointment.BehaviorNotes;
            }
            else
            {
                appointment.BehaviorNotes = appointment.BehaviorNotes;
            }
            if (newAppointment.AdditionalServices != null)
            {
                appointment.AdditionalServices = newAppointment.AdditionalServices;
            }
            else
            {
                appointment.AdditionalServices = appointment.AdditionalServices;
            }
            if (newAppointment.OwnerDebrief != null)
            {
                appointment.OwnerDebrief = newAppointment.OwnerDebrief;
            }
            else
            {
                appointment.OwnerDebrief = appointment.OwnerDebrief;
            }
            if (newAppointment.Notes != null)
            {
                appointment.Notes = newAppointment.Notes;
            }
            else
            {
                appointment.Notes = appointment.Notes;
            }
            if (newAppointment.TotalCost != 0)
            {
                appointment.TotalCost = newAppointment.TotalCost;
            }
            else
            {
                appointment.TotalCost = appointment.TotalCost;
            }
            if (newAppointment.Date != null)
            {
                appointment.Date = newAppointment.Date;
            }
            else
            {
                appointment.Date = appointment.Date;
            }

            _ctx.Appointments.Update(appointment);
            await _ctx.SaveChangesAsync();
            return appointment;
        }
    }
}
