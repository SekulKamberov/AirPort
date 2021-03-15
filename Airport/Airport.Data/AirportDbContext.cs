namespace Airport.Data
{  
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using Data.Models;

    public class AirportDbContext : IdentityDbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> options)
            : base(options)
        {

        }

        public DbSet<RegularUser> RegularUsers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Route> Routes { get; set; }

        public virtual DbSet<Airport> Airports { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Town>()
                .HasMany(t => t.Airports)
                .WithOne(a => a.Town)
                .HasForeignKey(a => a.TownId);

            builder.Entity<Town>()
                .HasMany(t => t.Companies)
                .WithOne(c => c.Town)
                .HasForeignKey(c => c.TownId);

            builder.Entity<Company>()
                .HasBaseType<User>();

            builder.Entity<Company>()
                .HasIndex(c => c.UniqueReferenceNumber)
                .IsUnique();

            builder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<Company>()
                .HasMany(c => c.Routes)
                .WithOne(r => r.Company)
                .HasForeignKey(r => r.CompanyId);

            builder.Entity<Route>()
                 .HasOne(r => r.StartAirport)
                 .WithMany(s => s.DepartureRoutes)
                 .HasForeignKey(r => r.StartAirportId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Route>()
                 .HasOne(r => r.EndAirport)
                 .WithMany(s => s.ArrivalRoutes)
                 .HasForeignKey(r => r.EndAirportId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                 .HasOne(r => r.User)
                 .WithMany(u => u.Reviews)
                 .HasForeignKey(r => r.UserId);

            builder.Entity<Review>()
                 .HasOne(r => r.Company)
                 .WithMany(u => u.Reviews)
                 .HasForeignKey(r => r.CompanyId);

            builder.Entity<Ticket>()
                 .HasOne(t => t.User)
                 .WithMany(u => u.Tickets)
                 .HasForeignKey(r => r.UserId);

            builder.Entity<Ticket>()
                 .HasOne(t => t.Route)
                 .WithMany(r => r.Tickets)
                 .HasForeignKey(r => r.RouteId);

            builder.Entity<RegularUser>()
                 .HasBaseType<User>();

            base.OnModelCreating(builder);
        }
    }
}
