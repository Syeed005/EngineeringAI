using EngineeringAI.Application.Common;
using EngineeringAI.Application.DTOs.Equipment;
using EngineeringAI.Application.Exceptions;
using EngineeringAI.Application.Interfaces.Messaging;
using EngineeringAI.Application.Interfaces.Repositories;
using EngineeringAI.Application.Interfaces.Services;
using EngineeringAI.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Services {
    public class EquipmentService : IEquipmentService {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IEquipmentEventPublisher _eventPublisher;
        private readonly ILogger<EquipmentService> _logger;
        public EquipmentService(IEquipmentRepository repository, ILogger<EquipmentService> logger, IEquipmentEventPublisher eventPublisher) {
            _equipmentRepository = repository;
            _logger = logger;
            _eventPublisher = eventPublisher;
        }

        public async Task<PagedResult<EquipmentDto>> GetAsync(EquipmentQueryParameters queryParameters, CancellationToken cancellationToken = default) {
            var result = await _equipmentRepository.GetAsync(queryParameters, cancellationToken);

            return new PagedResult<EquipmentDto> {
                Items = result.Items.Select(MapToDto).ToList(),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }

        public async Task<EquipmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
            var equipment = await _equipmentRepository.GetByIdAsync(id, cancellationToken);
            return equipment == null ? null : MapToDto(equipment);
        }

        public async Task<IReadOnlyList<EquipmentDto>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default) {
            var equipment = await _equipmentRepository.GetByProjectIdAsync(projectId, cancellationToken);
            return equipment.Select(MapToDto).ToList();
        }

        public async Task<EquipmentDto> CreateAsync(CreateEquipmentRequest request, CancellationToken cancellationToken = default) {
            _logger.LogInformation("Creating equipment {EquipmentNumber} for ProjectId {ProjectId}.", request.EquipmentNumber, request.ProjectId);

            var exists = await _equipmentRepository.ExistsByEquipmentNumberAsync(request.EquipmentNumber, cancellationToken);

            if (exists) {
                _logger.LogWarning("Duplicate equipment number detected: {EquipmentNumber}.", request.EquipmentNumber);
                throw new ConflictException($"Equipment number '{request.EquipmentNumber}' already exists.");
            }
                

            var projectExists = await _equipmentRepository.ProjectExistsAsync(request.ProjectId, cancellationToken);

            if (!projectExists) {
                _logger.LogWarning("Project {ProjectId} was not found while creating equipment {EquipmentNumber}.", request.ProjectId, request.EquipmentNumber);
                throw new NotFoundException($"Project with Id '{request.ProjectId}' was not found.");
            }
                

            if (request.SupplierId.HasValue) {
                var supplierExists = await _equipmentRepository.SupplierExistsAsync(request.SupplierId.Value, cancellationToken);

                if (!supplierExists) {
                    _logger.LogWarning("Supplier with Id {SupplierId} was not found while creating equipment {EquipmentNumber}.", request.SupplierId.Value, request.EquipmentNumber);
                    throw new NotFoundException($"Supplier with Id '{request.SupplierId.Value}' was not found.");
                }
                    
            }

            
            var equipment = new Equipment {
                EquipmentNumber = request.EquipmentNumber.Trim(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                ProjectId = request.ProjectId,
                SupplierId = request.SupplierId,
                Manufacturer = request.Manufacturer?.Trim(),
                EquipmentType = request.EquipmentType?.Trim(),
                Status = request.Status.Trim(),
                CreatedDate = DateTime.UtcNow
            };

            var created = await _equipmentRepository.AddAsync(equipment, cancellationToken);
            _logger.LogInformation("Equipment {EquipmentNumber} created successfully with Id {EquipmentId}.", equipment.EquipmentNumber, equipment.Id);
                  
            await _eventPublisher.PublishEquipmentCreatedAsync(
                equipment.Id,
                equipment.EquipmentNumber,
                equipment.ProjectId,
                cancellationToken);

            return MapToDto(created);
        }

        public async Task<EquipmentDto> UpdateAsync(int id, UpdateEquipmentRequest request, CancellationToken cancellationToken = default) {
            _logger.LogInformation("Updating equipment Id {EquipmentId}.", id);
            var equipment = await _equipmentRepository.GetByIdAsync(id, cancellationToken);

            if (equipment == null) {
                _logger.LogWarning("Equipment with Id {EquipmentId} was not found for update.", id);
                throw new NotFoundException($"Equipment with Id '{id}' was not found.");
            }
                

            var duplicateExists = await _equipmentRepository.ExistsByEquipmentNumberAsync(request.EquipmentNumber, id, cancellationToken);

            if (duplicateExists) {
                _logger.LogWarning("Duplicate equipment number detected during update: {EquipmentNumber}.", request.EquipmentNumber);
                throw new ConflictException($"Equipment number '{request.EquipmentNumber}' already exists.");
            }
                

            var projectExists = await _equipmentRepository.ProjectExistsAsync(request.ProjectId, cancellationToken);

            if (!projectExists) {
                _logger.LogWarning("Project {ProjectId} was not found while updating equipment Id {EquipmentId}.", request.ProjectId, id);
                throw new NotFoundException($"Project with Id '{request.ProjectId}' was not found.");
            }
                

            if (request.SupplierId.HasValue) {
                var supplierExists = await _equipmentRepository.SupplierExistsAsync(request.SupplierId.Value, cancellationToken);

                if (!supplierExists) {
                    _logger.LogWarning("Supplier with Id {SupplierId} was not found while updating equipment Id {EquipmentId}.", request.SupplierId.Value, id);
                    throw new NotFoundException($"Supplier with Id '{request.SupplierId.Value}' was not found.");
                }
                    
            }

            equipment.EquipmentNumber = request.EquipmentNumber.Trim();
            equipment.Name = request.Name.Trim();
            equipment.Description = request.Description?.Trim();
            equipment.ProjectId = request.ProjectId;
            equipment.SupplierId = request.SupplierId;
            equipment.Manufacturer = request.Manufacturer?.Trim();
            equipment.EquipmentType = request.EquipmentType?.Trim();
            equipment.Status = request.Status.Trim();
            equipment.ModifiedDate = DateTime.UtcNow;

            await _equipmentRepository.UpdateAsync(equipment, cancellationToken);
            _logger.LogInformation("Equipment Id {EquipmentId} updated successfully.", id);
            return MapToDto(equipment);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default) {
            _logger.LogInformation("Attempting to delete equipment Id {EquipmentId}.", id);
            var equipment = await _equipmentRepository.GetByIdAsync(id, cancellationToken);

            if (equipment == null) {
                _logger.LogWarning("Equipment with Id {EquipmentId} was not found.", id);
                throw new NotFoundException($"Equipment with Id '{id}' was not found.");
            }

            var hasDeliverables = await _equipmentRepository.HasDeliverablesAsync(id, cancellationToken);

            if (hasDeliverables) {
                _logger.LogWarning("Equipment with Id {EquipmentId} cannot be deleted because related deliverables exist.", id);
                throw new ConflictException($"Equipment with Id '{id}' cannot be deleted because related deliverables exist.");
            }

            await _equipmentRepository.DeleteAsync(equipment, cancellationToken);
            _logger.LogInformation("Deleting equipment Id {EquipmentId}.", id);
        }

        // No AutoMapper yet. Manual mapping makes the architecture obvious, gives us complete control, and avoids another dependency before we actually need one.
        private static EquipmentDto MapToDto(Equipment equipment) {
            return new EquipmentDto {
                Id = equipment.Id,
                EquipmentNumber = equipment.EquipmentNumber,
                Name = equipment.Name,
                Description = equipment.Description,
                ProjectId = equipment.ProjectId,
                SupplierId = equipment.SupplierId,
                Manufacturer = equipment.Manufacturer,
                EquipmentType = equipment.EquipmentType,
                Status = equipment.Status,
                CreatedDate = equipment.CreatedDate,
                ModifiedDate = equipment.ModifiedDate
            };
        }
    }
}
