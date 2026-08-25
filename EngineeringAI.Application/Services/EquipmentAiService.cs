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

                Identify:
                - a concise engineering summary,
                - missing information,
                - potential risks,
                - recommended follow-up actions.
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

            var analysis = await _aiClient.GenerateStructuredAsync<EquipmentAiAnalysis>(systemPrompt,userPrompt,cancellationToken);

            return new EquipmentAiSummaryResponse {
                EquipmentId = equipmentId,
                Summary = analysis.Summary,
                MissingInformation = analysis.MissingInformation,
                Risks = analysis.Risks,
                RecommendedActions = analysis.RecommendedActions
            };
        }
    }
}
