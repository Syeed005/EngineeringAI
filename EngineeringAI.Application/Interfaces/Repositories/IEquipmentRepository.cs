using EngineeringAI.Application.Common;
using EngineeringAI.Application.DTOs.Equipment;
using EngineeringAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.Repositories {
    public interface IEquipmentRepository {
        Task<PagedResult<Equipment>> GetAsync(EquipmentQueryParameters queryParameters, CancellationToken cancellationToken = default);

        Task<Equipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Equipment>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEquipmentNumberAsync(string equipmentNumber, CancellationToken cancellationToken = default);

        Task<Equipment> AddAsync(Equipment equipment, CancellationToken cancellationToken = default);
        Task<bool> ProjectExistsAsync(int projectId, CancellationToken cancellationToken = default);

        Task<bool> SupplierExistsAsync(int supplierId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEquipmentNumberAsync(string equipmentNumber, int excludeEquipmentId, CancellationToken cancellationToken = default);

        Task UpdateAsync(Equipment equipment, CancellationToken cancellationToken = default);
        Task DeleteAsync(Equipment equipment, CancellationToken cancellationToken = default);

        Task<bool> HasDeliverablesAsync(int equipmentId, CancellationToken cancellationToken = default);
    }
}
