using FinanceApp.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceApp.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(c => c.Status).HasColumnName("status").HasConversion<string>();
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
        builder.Property(c => c.UpdatedAt).HasColumnName("updated_at");
        builder.Property(c => c.VerifiedAt).HasColumnName("verified_at");

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value).HasColumnName("email").HasMaxLength(255).IsRequired();
            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.OwnsOne(c => c.CPF, cpf =>
        {
            cpf.Property(e => e.Value).HasColumnName("cpf").HasMaxLength(11).IsRequired();
            cpf.HasIndex(e => e.Value).IsUnique();
        });

        builder.OwnsOne(c => c.PhoneNumber, phone =>
        {
            phone.Property(e => e.Value).HasColumnName("phone_number").HasMaxLength(11).IsRequired();
        });
    }
}
