using EngineeringAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Data.Configurations {
    public class DeliverableConfiguration : IEntityTypeConfiguration<Deliverable> {
        public void Configure(EntityTypeBuilder<Deliverable> builder) {
            builder.ToTable("Deliverables");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DocumentNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(x => x.Revision)
                .HasMaxLength(20);

            builder.Property(x => x.DocumentType)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.FileName)
                .HasMaxLength(255);

            builder.Property(x => x.FilePath)
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.DocumentNumber,
                x.Revision
            })
            .IsUnique();

            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.EquipmentId);
            builder.HasIndex(x => x.DocumentType);
            builder.HasIndex(x => x.Status);
        }
    }
}
