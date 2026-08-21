using EngineeringAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Data.Configurations {
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment> {
        public void Configure(EntityTypeBuilder<Equipment> builder) {
            builder.ToTable("Equipment");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EquipmentNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.EquipmentNumber)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Manufacturer)
                .HasMaxLength(200);

            builder.Property(x => x.EquipmentType)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.SupplierId);
            builder.HasIndex(x => x.EquipmentType);
            builder.HasIndex(x => x.Status);

            builder.HasMany(x => x.Deliverables)
                .WithOne(x => x.Equipment)
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
