using EngineeringAI.Application.DTOs.AI;
using EngineeringAI.Application.Exceptions;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EngineeringAI.Application.Services {
    public class EquipmentAiService {
        private readonly IEquipmentRepository _repository;
        private readonly IEngineeringAiClient _aiClient;

        public EquipmentAiService(IEquipmentRepository repository, IEngineeringAiClient aiClient) {
            _repository = repository;
            _aiClient = aiClient;
        }

        public async Task<EquipmentAiSummaryResponse> GenerateSummaryAsync(int equipmentId, CancellationToken cancellationToken = default) {
            var equipment = await _repository.GetByIdAsync(equipmentId, cancellationToken);

            if (equipment is null) {
                throw new NotFoundException($"Equipment with Id '{equipmentId}' was not found.");
            }

            var systemPrompt =
                """
                You are an engineering assistant.
                Analyze the supplied equipment information.
                Do not invent facts that are not present in the supplied data.
                Return ONLY valid JSON using exactly this structure:
                {
                  "summary": "string",
                  "missingInformation": ["string"],
                  "risks": ["string"],
                  "recommendedActions": ["string"]
                }
                If no items exist for an array, return an empty array.
                """;

            var userPrompt =
                $"""
                Equipment information:

                Equipment Number: {equipment.EquipmentNumber}
                Name: {equipment.Name}
                Description: {equipment.Description}
                Equipment Type: {equipment.EquipmentType}
                Manufacturer: {equipment.Manufacturer}
                Status: {equipment.Status}
                Project Id: {equipment.ProjectId}
                Supplier Id: {equipment.SupplierId}                
                """;

            var aiResponse = await _aiClient.GenerateAsync(systemPrompt, userPrompt, cancellationToken);

            var result = JsonSerializer.Deserialize<EquipmentAiSummaryResponse>(
                aiResponse,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                });

            if (result is null) {
                throw new InvalidOperationException("AI response could not be deserialized.");
            }

            result.EquipmentId = equipmentId;

            return result;
        }
    }
}
