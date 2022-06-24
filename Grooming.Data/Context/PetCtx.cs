using Grooming.Data.Model;
using Grooming.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Grooming.Data.Context
{
    /// <summary>
    /// Pet database context provider.
    /// </summary>
    public class PetCtx : DbContext, IPetCtx
    {
        public PetCtx(DbContextOptions<PetCtx> options) : base(options)
        { }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SeedData();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
