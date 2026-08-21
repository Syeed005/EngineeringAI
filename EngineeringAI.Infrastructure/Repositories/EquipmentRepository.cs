using EngineeringAI.Application.Common;
using EngineeringAI.Application.DTOs.Equipment;
using EngineeringAI.Application.Interfaces.Repositories;
using EngineeringAI.Domain.Entities;
using EngineeringAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Repositories {
    public class EquipmentRepository : IEquipmentRepository {
        private readonly EngineeringDbContext _dbContext;
        public EquipmentRepository(EngineeringDbContext dbContext) {
            _dbContext = dbContext;
        }
        
        public async Task<PagedResult<Equipment>> GetAsync(EquipmentQueryParameters queryParameters, CancellationToken cancellationToken = default) {

            /*does not immediately query SQL Server. We are building the query step by step:
            
            AsNoTracking() is appropriate because these are read-only queries. EF Core doesn't need to track the returned entities for changes, which reduces unnecessary tracking overhead.

            We're also passing CancellationToken all the way down to EF Core
            So if the API receives:

                /api/equipment?projectId=2&equipmentType=Pump&pageNumber=1&pageSize=10
                
                EF Core can translate that into SQL conceptually like:
                
                SELECT ...
                FROM Equipment
                WHERE ProjectId = 2
                  AND EquipmentType = 'Pump'
                ORDER BY Id
                OFFSET 0 ROWS
                FETCH NEXT 10 ROWS ONLY;
             */

            //already gives you an IQueryable<Equipment>.
            var query = _dbContext.Equipment.AsNoTracking();

            
            //filtering
            if (queryParameters.ProjectId.HasValue)
                query = query.Where(x => x.ProjectId == queryParameters.ProjectId.Value);

            if (queryParameters.SupplierId.HasValue)
                query = query.Where(x => x.SupplierId == queryParameters.SupplierId.Value);

            if (!string.IsNullOrWhiteSpace(queryParameters.EquipmentType))
                query = query.Where(x => x.EquipmentType == queryParameters.EquipmentType);

            if (!string.IsNullOrWhiteSpace(queryParameters.Status))
                query = query.Where(x => x.Status == queryParameters.Status);

            //search -One important production point, though: our current Contains() implementation is perfectly fine for our 200-row learning dataset, but I would not automatically use this exact design for millions of production rows. Searching several columns with %term% can become expensive and may prevent effective index seeks. At that scale we'd evaluate full-text search, a dedicated search service, or—in our eventual architecture—Azure AI Search.

            if (!string.IsNullOrWhiteSpace(queryParameters.Search)) {
                var search = queryParameters.Search.Trim();

                query = query.Where(x =>
                    x.EquipmentNumber.Contains(search) ||
                    x.Name.Contains(search) ||
                    (x.Description != null && x.Description.Contains(search)) ||
                    (x.Manufacturer != null && x.Manufacturer.Contains(search)) ||
                    (x.EquipmentType != null && x.EquipmentType.Contains(search)));
            }

            // Count matching records before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            //sorting
            var descending = string.Equals(queryParameters.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

            query = queryParameters.SortBy?.ToLowerInvariant() switch {
                "name" => descending
                    ? query.OrderByDescending(x => x.Name).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.Name).ThenBy(x => x.Id),

                "equipmentnumber" => descending
                    ? query.OrderByDescending(x => x.EquipmentNumber).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.EquipmentNumber).ThenBy(x => x.Id),

                "equipmenttype" => descending
                    ? query.OrderByDescending(x => x.EquipmentType).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.EquipmentType).ThenBy(x => x.Id),

                "status" => descending
                    ? query.OrderByDescending(x => x.Status).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.Status).ThenBy(x => x.Id),

                "createddate" => descending
                    ? query.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Id)
                    : query.OrderBy(x => x.CreatedDate).ThenBy(x => x.Id),

                _ => query.OrderBy(x => x.Id)
            };

            //paginaiton
            var items = await query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)queryParameters.PageSize);

            return new PagedResult<Equipment> {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<Equipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _dbContext.Equipment
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        }

        public async Task<IReadOnlyList<Equipment>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default) {
            return await _dbContext.Equipment
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.EquipmentNumber)
            .ToListAsync(cancellationToken);
        }

        

        public async Task<bool> ExistsByEquipmentNumberAsync(string equipmentNumber, CancellationToken cancellationToken = default) {
            return await _dbContext.Equipment.AnyAsync(x => x.EquipmentNumber == equipmentNumber, cancellationToken);
        }

        public async Task<Equipment> AddAsync(Equipment equipment, CancellationToken cancellationToken = default) {
            await _dbContext.Equipment.AddAsync(equipment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return equipment;
        }

        public async Task<bool> ProjectExistsAsync(int projectId, CancellationToken cancellationToken = default) {
            return await _dbContext.Projects.AnyAsync(x => x.Id == projectId, cancellationToken);
        }

        public async Task<bool> SupplierExistsAsync(int supplierId, CancellationToken cancellationToken = default) {
            return await _dbContext.Suppliers.AnyAsync(x => x.Id == supplierId, cancellationToken);
        }

        public async Task<bool> ExistsByEquipmentNumberAsync(string equipmentNumber, int excludeEquipmentId, CancellationToken cancellationToken = default) {
            return await _dbContext.Equipment.AnyAsync(x => x.EquipmentNumber == equipmentNumber && x.Id != excludeEquipmentId, cancellationToken);
        }

        public async Task UpdateAsync(Equipment equipment, CancellationToken cancellationToken = default) {
            _dbContext.Equipment.Update(equipment);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Equipment equipment, CancellationToken cancellationToken = default) {
            _dbContext.Equipment.Remove(equipment);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> HasDeliverablesAsync(int equipmentId, CancellationToken cancellationToken = default) {
            return await _dbContext.Deliverables.AnyAsync(x => x.EquipmentId == equipmentId, cancellationToken);
        }
    }
}
