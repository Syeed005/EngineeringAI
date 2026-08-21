using EngineeringAI.Application.DTOs.Equipment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Validation.Equipment {
    public class CreateEquipmentRequestValidator : AbstractValidator<CreateEquipmentRequest> {
        //This handles request-shape validation.
        public CreateEquipmentRequestValidator() {
            RuleFor(x => x.EquipmentNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.ProjectId)
                .GreaterThan(0);

            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .When(x => x.SupplierId.HasValue);

            RuleFor(x => x.Manufacturer)
                .MaximumLength(200);

            RuleFor(x => x.EquipmentType)
                .MaximumLength(100);

            RuleFor(x => x.Status)
                .NotEmpty()
                .MaximumLength(50);
            //later we may move these values into enums or reference tables. For now, string + validator is easier because your database already stores these values as NVARCHAR.

            var allowedStatuses = new[]
            {
                "Planned",
                "In Review",
                "Approved",
                "Installed",
                "Inactive"
            };

            var allowedEquipmentTypes = new[]
            {
                "Pump",
                "Motor",
                "Valve",
                "Conveyor",
                "Heat Exchanger",
                "Tank",
                "Compressor",
                "Instrument",
                "Electrical Panel"
            };

            RuleFor(x => x.Status)
                .NotEmpty()
                .MaximumLength(50)
                .Must(x => allowedStatuses.Contains(x))
                .WithMessage("Status must be one of: Planned, In Review, Approved, Installed, Inactive.");

            RuleFor(x => x.EquipmentType)
                .MaximumLength(100)
                .Must(x => string.IsNullOrWhiteSpace(x) || allowedEquipmentTypes.Contains(x))
                .WithMessage("EquipmentType contains an unsupported value.");
        }
    }
}
