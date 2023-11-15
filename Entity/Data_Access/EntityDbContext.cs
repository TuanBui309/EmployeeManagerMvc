using Entity.Models;
using Microsoft.EntityFrameworkCore;
namespace Entity.Data_Access
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Warns { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Nation> Nations { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DistictName)
                  .IsRequired()
                  .HasColumnName("DistictName")
                  .HasMaxLength(350)
                  .IsUnicode(true);
                entity.Property(e => e.CityId).HasColumnName("CityId");
                entity.HasOne(d => d.City)
                    .WithMany(x => x.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_provice_id");
                ;
            });
            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("Ward");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.WardName)
                  .IsRequired()
                  .HasColumnName("WardName")
                  .HasMaxLength(350)
                  .IsUnicode(true);
                entity.Property(e => e.DistrictId).HasColumnName("DistrictId");
                entity.HasOne(d => d.District)
                    .WithMany(x => x.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_district_id");
            });
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CityName)
                  .IsRequired()
                  .HasColumnName("CityName")
                  .HasMaxLength(350)
                  .IsUnicode(true);
            }
            );
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(x => x.Ward).WithMany()
                    .HasForeignKey(x => x.WardId)
                     .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.District).WithMany()
                    .HasForeignKey(x => x.DistrictId)
                    .HasConstraintName("fk_DS")
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.City).WithMany()
                    .HasForeignKey(x => x.CityId)
                    .HasConstraintName("fk_city");
                entity.HasOne(x => x.Job).WithMany()
                    .HasForeignKey(x => x.JobId)
                    .HasConstraintName("job");
                entity.HasOne(x => x.Nation).WithMany()
                    .HasForeignKey(x => x.NationId)
                    .HasConstraintName("fk_na");
            });
            modelBuilder.Entity<Degree>(entity =>
            {
                entity.HasOne(x => x.employee).WithMany()
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_employee_id");

            });

        }
    }
}
