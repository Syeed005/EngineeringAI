using EngineeringAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Data {
    public class EngineeringDbContext: DbContext {
        public EngineeringDbContext(DbContextOptions<EngineeringDbContext> options) : base(options) {
            
        }
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Equipment> Equipment => Set<Equipment>();
        public DbSet<Deliverable> Deliverables => Set<Deliverable>();
        public DbSet<IntegrationAudit> IntegrationAudits => Set<IntegrationAudit>();

        //That tells EF Core to automatically find our configuration classes instead of putting all database mapping inside OnModelCreating().
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(EngineeringDbContext).Assembly);
        }



    }
}
