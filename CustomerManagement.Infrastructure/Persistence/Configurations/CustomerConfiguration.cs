using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerManagement.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.Document)
                .IsRequired();
                
            builder.Property(c => c.Phone)
                .IsRequired();
                
            builder.Property(c => c.RegistrationDate)
                .IsRequired();
                
            builder.Property(c => c.Type)
                .IsRequired();
                
            // Se você precisar de índices
            builder.HasIndex(c => c.Document);
            
            // Se você tiver relacionamentos, configure-os aqui
            // builder.HasMany(...)
        }
    }
} 