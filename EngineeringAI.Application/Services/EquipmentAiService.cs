using EngineeringAI.Application.Exceptions;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Services {
    public class EquipmentAiService {
        private readonly IEquipmentRepository _repository;
        private readonly IEngineeringAiClient _aiClient;

        public EquipmentAiService(IEquipmentRepository repository, IEngineeringAiClient aiClient) {
            _repository = repository;
            _aiClient = aiClient;
        }

        public async Task<string> GenerateSummaryAsync(int equipmentId, CancellationToken cancellationToken = default) {
            var equipment = await _repository.GetByIdAsync(equipmentId, cancellationToken);

            if (equipment is null) {
                throw new NotFoundException($"Equipment with Id '{equipmentId}' was not found.");
            }

            var systemPrompt =
                """
                You are an engineering assistant.
                Summarize equipment information clearly and professionally.
                Do not invent facts that are not present in the supplied data.
                Highlight important engineering details and any obvious missing information.
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

                Provide a concise engineering summary.
                """;

            return await _aiClient.GenerateAsync(systemPrompt, userPrompt, cancellationToken);
        }
    }
}
