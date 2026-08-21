using EngineeringAI.Application.DTOs.Equipment;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EngineeringAI.Tests.Integration {
    public class EquipmentApiTests : IClassFixture<CustomWebApplicationFactory> {
        private readonly HttpClient _client;

        public EquipmentApiTests(CustomWebApplicationFactory factory) {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetEquipment_ShouldReturnOk() {
            // Act
            var response = await _client.GetAsync("/api/equipment");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetEquipment_ShouldReturnSeededEquipment() {
            // Act
            var response = await _client.GetAsync("/api/equipment");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("EQ-TEST-001");
            content.Should().Contain("EQ-TEST-002");
            content.Should().Contain("EQ-TEST-003");
        }

        [Fact]
        public async Task GetEquipmentById_WhenEquipmentDoesNotExist_ShouldReturnNotFound() {
            // Act
            var response = await _client.GetAsync("/api/equipment/99999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateEquipment_WhenRequestIsValid_ShouldReturnCreated() {
            // Arrange
            var request = new {
                equipmentNumber = "EQ-TEST-100",
                name = "Integration Test Pump",
                description = "Created during integration testing.",
                projectId = 1,
                supplierId = 1,
                manufacturer = "Test Manufacturer",
                equipmentType = "Pump",
                status = "Planned"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/equipment", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdEquipment = await response.Content.ReadFromJsonAsync<EquipmentDto>();

            createdEquipment.Should().NotBeNull();
            createdEquipment!.EquipmentNumber.Should().Be("EQ-TEST-100");

            // Cleanup
            var deleteResponse = await _client.DeleteAsync($"/api/equipment/{createdEquipment.Id}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
