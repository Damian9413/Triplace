using Triplace.Application.Commands;
using Triplace.Application.Repositories;
using Triplace.Domain.Builders;
using Triplace.Domain.Entities;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;

namespace Triplace.Application.Services;

public class AttractionGroupService(
    IAttractionGroupRepository groupRepository,
    IAttractionRepository attractionRepository,
    IAttractionAddonTypeRepository addonTypeRepository)
{
    public async Task<AttractionGroupId> CreateAsync(CreateAttractionGroupCommand command)
    {
        var group = AttractionGroupBuilder.Create(command.Name).Build();
        await groupRepository.SaveAsync(group);
        return group.Id;
    }

    public async Task AddEntryAsync(AddGroupEntryCommand command)
    {
        var group = await GetGroupOrThrowAsync(command.GroupId);
        var node = await ResolveNodeAsync(command.NodeId, command.NodeType);
        var entry = group.AddEntry(node);

        if (command.Addons != null)
        {
            foreach (var addonCmd in command.Addons)
            {
                var addonType = await addonTypeRepository.GetByIdAsync(
                    new AttractionAddonTypeId(addonCmd.AddonTypeId));
                if (addonType is null)
                    throw new EntityNotFoundException($"AddonType {addonCmd.AddonTypeId} not found.");
                entry.AddAddon(addonType, addonCmd.Values);
            }
        }

        await groupRepository.SaveAsync(group);
    }

    public async Task<AttractionGroup?> GetByIdAsync(AttractionGroupId id)
        => await groupRepository.GetByIdAsync(id);

    public async Task<IReadOnlyList<Attraction>> FlattenAsync(AttractionGroupId id)
    {
        var group = await GetGroupOrThrowAsync(id);
        return group.Flatten();
    }

    private async Task<AttractionGroup> GetGroupOrThrowAsync(AttractionGroupId id)
    {
        var group = await groupRepository.GetByIdAsync(id);
        if (group is null)
            throw new EntityNotFoundException($"AttractionGroup {id.Value} not found.");
        return group;
    }

    private async Task<Domain.Interfaces.IAttractionNode> ResolveNodeAsync(Guid nodeId, string nodeType)
    {
        if (nodeType.Equals("attraction", StringComparison.OrdinalIgnoreCase))
        {
            var attraction = await attractionRepository.GetByIdAsync(new AttractionId(nodeId));
            if (attraction is null)
                throw new EntityNotFoundException($"Attraction {nodeId} not found.");
            return attraction;
        }
        else if (nodeType.Equals("group", StringComparison.OrdinalIgnoreCase))
        {
            var group = await groupRepository.GetByIdAsync(new AttractionGroupId(nodeId));
            if (group is null)
                throw new EntityNotFoundException($"AttractionGroup {nodeId} not found.");
            return group;
        }
        throw new DomainException($"Unknown node type: '{nodeType}'. Use 'attraction' or 'group'.");
    }
}
