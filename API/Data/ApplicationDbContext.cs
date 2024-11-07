using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> AppUsers {get; set;}
    public DbSet<ServiceRequest> ServiceRequests {get; set;}
    public DbSet<Technician> Technicians { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //configuring the foreign key relationshipbetween ServiceRequest and Technician
        modelBuilder.Entity<ServiceRequest>()
        .HasOne(sr => sr.Technician)
        .WithMany() //Assuming one to many relationship: a technician can have a multiple requests
        .HasForeignKey(sr => sr.TechnicianId)
        .OnDelete(DeleteBehavior.SetNull); //if a technician is deleted, set TechnicianId to null
    }
}


