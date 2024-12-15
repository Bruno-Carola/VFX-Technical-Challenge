using Microsoft.EntityFrameworkCore;
using VFXFinancial.WebApi.Models.Domain;

namespace VFXFinancial.WebApi.Data
{
    /// <summary>
    ///   VFXFinancialDbContext
    /// </summary>
    public class VFXFinancialDbContext : DbContext
    {
        /// <summary>Initializes a new instance of the <see cref="VFXFinancialDbContext" /> class.</summary>
        /// <param name="options">The options.</param>
        public VFXFinancialDbContext(DbContextOptions<VFXFinancialDbContext> options) :base(options) { }

        /// <summary>Gets or sets the exchange rates.</summary>
        /// <value>The exchange rates.</value>
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRate>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.FromCurrency)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.ToCurrency)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.Bid)
                .HasPrecision(18, 8); // Up to 8 decimal places

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.Ask)
                .HasPrecision(18, 8); // Up to 8 decimal places

            base.OnModelCreating(modelBuilder);
        }
    }
}
