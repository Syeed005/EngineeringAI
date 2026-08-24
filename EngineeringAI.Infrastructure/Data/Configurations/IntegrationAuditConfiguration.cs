using EngineeringAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Data.Configurations {
    public class IntegrationAuditConfiguration : IEntityTypeConfiguration<IntegrationAudit> {
        public void Configure(EntityTypeBuilder<IntegrationAudit> builder) {
            builder.ToTable("IntegrationAudit");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.EventType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.EquipmentNumber)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ProcessedAtUtc)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.EventId)
                .IsUnique();

            builder.HasIndex(x => x.EquipmentId);
        }
    }
}
