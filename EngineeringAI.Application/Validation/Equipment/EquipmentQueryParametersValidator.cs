using EngineeringAI.Application.DTOs.Equipment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Validation.Equipment {
    public class EquipmentQueryParametersValidator: AbstractValidator<EquipmentQueryParameters> {
        private static readonly string[] AllowedSortFields =
    {
        "name",
        "equipmentnumber",
        "equipmenttype",
        "status",
        "createddate"
    };

        private static readonly string[] AllowedSortDirections =
        {
        "asc",
        "desc"
    };

        public EquipmentQueryParametersValidator() {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.ProjectId)
                .GreaterThan(0)
                .When(x => x.ProjectId.HasValue);

            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .When(x => x.SupplierId.HasValue);

            RuleFor(x => x.SortBy)
                .Must(x => string.IsNullOrWhiteSpace(x) || AllowedSortFields.Contains(x.ToLowerInvariant()))
                .WithMessage("SortBy must be one of: name, equipmentNumber, equipmentType, status, createdDate.");

            RuleFor(x => x.SortDirection)
                .Must(x => string.IsNullOrWhiteSpace(x) || AllowedSortDirections.Contains(x.ToLowerInvariant()))
                .WithMessage("SortDirection must be either asc or desc.");
        }
    }
}
