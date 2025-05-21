using Microsoft.EntityFrameworkCore;

namespace CSO.Core.DatabaseContext;

public class CSOResponsiveDbContext : DbContext
{
    public CSOResponsiveDbContext(DbContextOptions<CSOResponsiveDbContext> options) : base(options)
    {

    }
    public DbSet<ExceptionLogger> ExceptionLoggers { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<Categorys> Categories { get; set; }
    public DbSet<Plant> Plants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ComplaintType> ComplaintTypes { get; set; }
    public DbSet<CSOClass> CSOClasses { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<NearestPlant> NearestPlants { get; set; }
    public DbSet<CSOLog> CSOLogs { get; set; }
    public DbSet<CSOLogFile> CSOLogFiles { get; set; }
    public DbSet<CSOLogHistory> CSOLogHistories { get; set; }
    public DbSet<MailMatrix> MailMatrices { get; set; }
    public DbSet<UsersRole> UserRoles { get; set; }
    public DbSet<RecycleDay> RecycleDays { get; set; }
    public DbSet<EmailConfiguration> EmailConfigurations { get; set; }
}
