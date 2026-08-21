using EngineeringAI.Application.Common;
using EngineeringAI.Application.DTOs.Equipment;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.Services {
    public interface IEquipmentService {
        Task<PagedResult<EquipmentDto>> GetAsync(EquipmentQueryParameters queryParameters, CancellationToken cancellationToken = default);

        Task<EquipmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EquipmentDto>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);

        Task<EquipmentDto> CreateAsync(CreateEquipmentRequest request, CancellationToken cancellationToken = default);
        Task<EquipmentDto> UpdateAsync(int id, UpdateEquipmentRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
