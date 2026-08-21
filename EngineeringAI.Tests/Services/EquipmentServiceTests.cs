using EngineeringAI.Application.DTOs.Equipment;
using EngineeringAI.Application.Exceptions;
using EngineeringAI.Application.Interfaces.Repositories;
using EngineeringAI.Application.Services;
using EngineeringAI.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Tests.Services {
    public class EquipmentServiceTests {
        [Fact]
        public async Task CreateAsync_WhenEquipmentNumberAlreadyExists_ShouldThrowConflictException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.ExistsByEquipmentNumberAsync("EQ-01001", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new CreateEquipmentRequest {
                EquipmentNumber = "EQ-01001",
                Name = "Test Pump",
                ProjectId = 1,
                Status = "Planned"
            };

            // Act
            Func<Task> act = async () => await service.CreateAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Equipment number 'EQ-01001' already exists.");
        }

        [Fact]
        public async Task CreateAsync_WhenRequestIsValid_ShouldCreateEquipment() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.ExistsByEquipmentNumberAsync("EQ-02001", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            repositoryMock
                .Setup(x => x.ProjectExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(x => x.SupplierExistsAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Equipment equipment, CancellationToken _) => {
                    equipment.Id = 1001;
                    return equipment;
                });

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new CreateEquipmentRequest {
                EquipmentNumber = "EQ-02001",
                Name = "Cooling Water Pump",
                Description = "Main cooling water pump",
                ProjectId = 1,
                SupplierId = 2,
                Manufacturer = "Apex Industrial",
                EquipmentType = "Pump",
                Status = "Planned"
            };

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();

            result.Id.Should().Be(1001);
            result.EquipmentNumber.Should().Be("EQ-02001");
            result.Name.Should().Be("Cooling Water Pump");
            result.ProjectId.Should().Be(1);
            result.SupplierId.Should().Be(2);
            result.EquipmentType.Should().Be("Pump");
            result.Status.Should().Be("Planned");
            repositoryMock.Verify(x => x.AddAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task CreateAsync_WhenProjectDoesNotExist_ShouldThrowNotFoundException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.ExistsByEquipmentNumberAsync("EQ-02002", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            repositoryMock
                .Setup(x => x.ProjectExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new CreateEquipmentRequest {
                EquipmentNumber = "EQ-02002",
                Name = "Test Pump",
                ProjectId = 999,
                Status = "Planned"
            };

            // Act
            Func<Task> act = async () => await service.CreateAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Project with Id '999' was not found.");
            repositoryMock.Verify(x => x.AddAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenSupplierDoesNotExist_ShouldThrowNotFoundException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.ExistsByEquipmentNumberAsync("EQ-02003", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            repositoryMock
                .Setup(x => x.ProjectExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(x => x.SupplierExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new CreateEquipmentRequest {
                EquipmentNumber = "EQ-02003",
                Name = "Test Motor",
                ProjectId = 1,
                SupplierId = 999,
                EquipmentType = "Motor",
                Status = "Planned"
            };

            // Act
            Func<Task> act = async () => await service.CreateAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Supplier with Id '999' was not found.");

            repositoryMock.Verify(x => x.AddAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenEquipmentDoesNotExist_ShouldThrowNotFoundException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Equipment?)null);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new UpdateEquipmentRequest {
                EquipmentNumber = "EQ-02004",
                Name = "Updated Pump",
                ProjectId = 1,
                SupplierId = 2,
                EquipmentType = "Pump",
                Status = "Approved"
            };

            // Act
            Func<Task> act = async () => await service.UpdateAsync(999, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Equipment with Id '999' was not found.");

            repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenEquipmentHasDeliverables_ShouldThrowConflictException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            var equipment = new Equipment {
                Id = 10,
                EquipmentNumber = "EQ-00010",
                Name = "Test Pump",
                ProjectId = 1,
                Status = "Approved"
            };

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipment);

            repositoryMock
                .Setup(x => x.HasDeliverablesAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            // Act
            Func<Task> act = async () => await service.DeleteAsync(10);

            // Assert
            await act.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Equipment with Id '10' cannot be deleted because related deliverables exist.");

            repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenEquipmentDoesNotExist_ShouldThrowNotFoundException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Equipment?)null);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            // Act
            Func<Task> act = async () => await service.DeleteAsync(999);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Equipment with Id '999' was not found.");

            repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenEquipmentIsValid_ShouldDeleteEquipment() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            var equipment = new Equipment {
                Id = 10,
                EquipmentNumber = "EQ-00010",
                Name = "Test Pump",
                ProjectId = 1,
                Status = "Approved"
            };

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipment);

            repositoryMock
                .Setup(x => x.HasDeliverablesAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            // Act
            await service.DeleteAsync(10);

            // Assert
            repositoryMock.Verify(x => x.DeleteAsync(equipment, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_WhenEquipmentNumberAlreadyExists_ShouldThrowConflictException() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            var equipment = new Equipment {
                Id = 10,
                EquipmentNumber = "EQ-00010",
                Name = "Existing Pump",
                ProjectId = 1,
                Status = "Planned"
            };

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipment);

            repositoryMock
                .Setup(x => x.ExistsByEquipmentNumberAsync("EQ-00020", 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new UpdateEquipmentRequest {
                EquipmentNumber = "EQ-00020",
                Name = "Updated Pump",
                ProjectId = 1,
                SupplierId = 2,
                EquipmentType = "Pump",
                Status = "Approved"
            };

            // Act
            Func<Task> act = async () => await service.UpdateAsync(10, request);

            // Assert
            await act.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Equipment number 'EQ-00020' already exists.");

            repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Equipment>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestIsValid_ShouldUpdateEquipment() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            var equipment = new Equipment {
                Id = 10,
                EquipmentNumber = "EQ-00010",
                Name = "Old Pump",
                ProjectId = 1,
                SupplierId = 1,
                EquipmentType = "Pump",
                Status = "Planned"
            };

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipment);

            repositoryMock
                .Setup(x => x.ExistsByEquipmentNumberAsync("EQ-00010", 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            repositoryMock
                .Setup(x => x.ProjectExistsAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(x => x.SupplierExistsAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            var request = new UpdateEquipmentRequest {
                EquipmentNumber = "EQ-00010",
                Name = "Updated Pump",
                Description = "Updated description",
                ProjectId = 2,
                SupplierId = 3,
                Manufacturer = "Apex Industrial",
                EquipmentType = "Pump",
                Status = "Approved"
            };

            // Act
            var result = await service.UpdateAsync(10, request);

            // Assert
            result.Id.Should().Be(10);
            result.Name.Should().Be("Updated Pump");
            result.ProjectId.Should().Be(2);
            result.SupplierId.Should().Be(3);
            result.Status.Should().Be("Approved");

            equipment.Name.Should().Be("Updated Pump");
            equipment.ProjectId.Should().Be(2);
            equipment.SupplierId.Should().Be(3);
            equipment.ModifiedDate.Should().NotBeNull();

            repositoryMock.Verify(x => x.UpdateAsync(equipment, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEquipmentExists_ShouldReturnEquipmentDto() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            var equipment = new Equipment {
                Id = 5,
                EquipmentNumber = "EQ-00005",
                Name = "Cooling Pump",
                ProjectId = 1,
                SupplierId = 2,
                EquipmentType = "Pump",
                Status = "Approved"
            };

            repositoryMock
                .Setup(x => x.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipment);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            // Act
            var result = await service.GetByIdAsync(5);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(5);
            result.EquipmentNumber.Should().Be("EQ-00005");
            result.Name.Should().Be("Cooling Pump");
            result.Status.Should().Be("Approved");
        }

        [Fact]
        public async Task GetByIdAsync_WhenEquipmentDoesNotExist_ShouldReturnNull() {
            // Arrange
            var repositoryMock = new Mock<IEquipmentRepository>();
            var loggerMock = new Mock<ILogger<EquipmentService>>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Equipment?)null);

            var service = new EquipmentService(repositoryMock.Object, loggerMock.Object);

            // Act
            var result = await service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }
    }
}
