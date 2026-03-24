using Triplace.Domain.Enums;
using Triplace.Domain.ValueObjects;

namespace Triplace.Application.Commands;

public record CreateAddonTypeCommand(string Name, IReadOnlyList<FieldDefinitionCommand> Fields);

public record FieldDefinitionCommand(string FieldName, FieldValueType ValueType, FieldConstraint Constraint);
