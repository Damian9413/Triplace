using Triplace.Application.Commands;
using Triplace.Application.Repositories;
using Triplace.Domain.Builders;
using Triplace.Domain.Entities;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;

namespace Triplace.Application.Services;

public class AttractionService(IAttractionRepository repository)
{
    public async Task<AttractionId> CreateDraftAsync(CreateAttractionCommand command)
    {
        var attraction = AttractionBuilder.CreateDraft(command.Name)
            .WithMetadata(m =>
            {
                foreach (var entry in command.MetadataEntries)
                    m.AddEntry(entry.Label, entry.Value);
            })
            .Build();

        await repository.SaveAsync(attraction);
        return attraction.Id;
    }

    public async Task PublishAsync(AttractionId id)
    {
        var attraction = await GetOrThrowAsync(id);
        attraction.Publish();
        await repository.SaveAsync(attraction);
    }

    public async Task ArchiveAsync(AttractionId id)
    {
        var attraction = await GetOrThrowAsync(id);
        attraction.Archive();
        await repository.SaveAsync(attraction);
    }

    public Task<Attraction?> GetByIdAsync(AttractionId id) => repository.GetByIdAsync(id);

    public Task<IReadOnlyList<Attraction>> GetAllAsync() => repository.GetAllAsync();

    private async Task<Attraction> GetOrThrowAsync(AttractionId id)
    {
        var attraction = await repository.GetByIdAsync(id);
        if (attraction is null)
            throw new EntityNotFoundException($"Attraction {id.Value} not found.");
        return attraction;
    }
}
