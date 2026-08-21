using EngineeringAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Data.Configurations {
    public class ProjectConfiguration : IEntityTypeConfiguration<Project> {
        public void Configure(EntityTypeBuilder<Project> builder) {
            builder.ToTable("Projects");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProjectNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.ProjectNumber)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Customer)
                .HasMaxLength(200);

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasMany(x => x.Equipment)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Deliverables)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
