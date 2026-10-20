using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Marketplace.Domain.Entities;

namespace Marketplace.Infrastructure.Persistence.Configurations;

public class AdConfiguration : IEntityTypeConfiguration<Ad>
{
    public void Configure(EntityTypeBuilder<Ad> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Price)
            .HasColumnType("decimal(18,2)");

        // ОПТИМИЗАЦИЯ ПРОИЗВОДИТЕЛЬНОСТИ: Создание индексов для устранения узкого места при фильтрации и поиске
        builder.HasIndex(a => a.SellerId);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => new { a.Status, a.CreatedAt });
    }
}