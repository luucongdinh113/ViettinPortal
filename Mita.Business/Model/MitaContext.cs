using log4net;
using Microsoft.EntityFrameworkCore;
using Mita.Business.Helpers;
using System;
using System.Data.Common;
using System.Linq;

namespace Mita.Business.Model
{
    public class MitaContext : DbContext
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string ConnectionString = string.Empty;

        public static MitaContext GetContextInstance()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                //string connectionString = "data source=(local);initial catalog=ViettinPortal;User ID=:userId;Password=:password;MultipleActiveResultSets=True;App=EntityFramework";
                //connectionString = connectionString.Replace(":userId", CommonUtils.Decrypt("SeAfZ3AKrqJqPielQ4ZcBg=="));
                //connectionString = connectionString.Replace(":password", CommonUtils.Decrypt("C91NRjnjPY2hip3/IgB4qA=="));
                string connectionString = "Data Source=DINHLC-03102001;Initial Catalog=ViettinPortal;Integrated Security=True;";
                ConnectionString = connectionString;
            }

            return new MitaContext(ConnectionString);
        }

        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<UserRight> UserRights { get; set; }
        public DbSet<ApplicationKey> ApplicationKeys { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<NodeMonitor> NodeMonitors { get; set; }
        public DbSet<NodeError> NodeErrors { get; set; }
        public DbSet<DailyMaintenanceBasic> DailyMaintenanceBasics { get; set; }
        public DbSet<StatMonthlyPatient> StatMonthlyPatients { get; set; }
        public DbSet<StatMonthlyResult> StatMonthlyResults { get; set; }
        public DbSet<StatMonthlyResultInstrument> StatMonthlyResultInstruments { get; set; }
        public DbSet<StatMonthlyResultInstrumentCombine> StatMonthlyResultInstrumentCombines { get; set; }
        public DbSet<StatMonthlyResultDepartment> StatMonthlyResultDepartments { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        public static DateTime GetCurrentTime()
        {
            using (var context = new MitaContext(ConnectionString))
            {
                using (var con = context.Database.GetDbConnection())
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandText = "SELECT GETDATE()";

                    var datetime = (DateTime)cmd.ExecuteScalar();
                    return datetime;
                }
            }
        }

        public MitaContext(DbContextOptions<MitaContext> options)
            : base(options)
        {

        }

        private MitaContext(string connectionString) : base(GetOptions(connectionString))
        {

            
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString)
                .Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRight>()
                .HasKey(c => new { c.UserId, c.RightCode });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseLoggerFactory()
        }
    }
}
