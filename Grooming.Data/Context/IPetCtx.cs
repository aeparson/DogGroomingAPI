using Grooming.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Grooming.Data.Context
{
    /// <summary>
    /// This interface provides an abstraction layer for the pet database context.
    /// </summary>
    public interface IPetCtx
    {
        public DbSet<Pet> Pets { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
