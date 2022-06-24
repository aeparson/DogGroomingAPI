using AutoMapper;
using Grooming.Data.Model;
using Grooming.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grooming.API.DTOMappings
{
    public static class MapperExtensions
    {

        public static IEnumerable<AppointmentDTO> MapAppointmentsToAppointmentDtos(this IMapper mapper, IEnumerable<Appointment> appointments)
        {
            return appointments
                .Select(x => mapper.MapAppointmentToAppointmentDto(x))
                .ToList();
        }

        /// <summary>
        /// Helper method to build model for a petAppointment DTO.
        /// </summary>
        /// <param name="petAppointment">The petAppointment to be persisted.</param>
        /// <returns>A petAppointment DTO.</returns>
        public static AppointmentDTO MapAppointmentToAppointmentDto(this IMapper mapper, Appointment appointment)
        {
            return new AppointmentDTO()
            {
                Id = appointment.Id,
                PetId = appointment.PetId,
                Shampoos = appointment.Shampoos,
                Conditioners = appointment.Conditioners,
                Groomer = appointment.Groomer,
                BathMethods = appointment.BathMethods,
                BrushInfo = appointment.BrushInfo,
                BehaviorNotes = appointment.BehaviorNotes,
                AdditionalServices = appointment.AdditionalServices,
                OwnerDebrief = appointment.OwnerDebrief,
                Notes = appointment.Notes,
                TotalCost = appointment.TotalCost,
                Date = appointment.Date
            };
        }

        public static Appointment MapAppointmentDtoToAppointment(this IMapper mapper, AppointmentDTO appointmentDTO)
        {
            var appointment = new Appointment
            {
                Date = DateTime.Today.ToString("yyyy-MM-dd"),
            };

            appointment.TotalCost = appointmentDTO.TotalCost;
            return appointment;

        }
    }
}
