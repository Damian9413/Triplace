using Triplace.Application.Commands;
using Triplace.Application.Repositories;
using Triplace.Domain.Builders;
using Triplace.Domain.Entities;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;

namespace Triplace.Application.Services;

public class AttractionAddonTypeService(IAttractionAddonTypeRepository repository)
{
    public async Task<AttractionAddonTypeId> CreateAsync(CreateAddonTypeCommand command)
    {
        var builder = AttractionAddonTypeBuilder.Create(command.Name);
        foreach (var field in command.Fields)
            builder.WithField(field.FieldName, field.ValueType, field.Constraint);

        var addonType = builder.Build();
        await repository.SaveAsync(addonType);
        return addonType.Id;
    }

    public Task<IReadOnlyList<AttractionAddonType>> GetAllAsync() => repository.GetAllAsync();

    public async Task<AttractionAddonType> GetByIdOrThrowAsync(AttractionAddonTypeId id)
    {
        var addonType = await repository.GetByIdAsync(id);
        if (addonType is null)
            throw new EntityNotFoundException($"AddonType {id.Value} not found.");
        return addonType;
    }

    public Task<AttractionAddonType?> GetByIdAsync(AttractionAddonTypeId id)
        => repository.GetByIdAsync(id);
}
