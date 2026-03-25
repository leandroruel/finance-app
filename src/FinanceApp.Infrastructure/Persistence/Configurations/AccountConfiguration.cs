using FinanceApp.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceApp.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.CustomerId).HasColumnName("customer_id");
        builder.Property(a => a.Type).HasColumnName("type").HasConversion<string>();
        builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>();
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at");

        builder.OwnsOne(a => a.Number, n =>
        {
            n.Property(x => x.Value).HasColumnName("number").HasMaxLength(10).IsRequired();
            n.HasIndex(x => x.Value).IsUnique();
        });

        builder.OwnsOne(a => a.Balance, b =>
        {
            b.Property(x => x.Amount).HasColumnName("balance_amount").HasPrecision(18, 2);
            b.Property(x => x.Currency).HasColumnName("balance_currency").HasMaxLength(3);
        });
    }
}
