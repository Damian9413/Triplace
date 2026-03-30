using Triplace.Application.Commands;
using Triplace.Application.Repositories;
using Triplace.Domain.Builders;
using Triplace.Domain.Entities;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;
using Triplace.Domain.Specifications;

namespace Triplace.Application.Services;

public class AttractionService(IAttractionRepository repository)
{
    public async Task<AttractionId> CreateDraftAsync(CreateAttractionCommand command)
    {
        var builder = AttractionBuilder.CreateDraft(command.Name)
            .InCategory(command.Category)
            .BestIn([.. command.BestSeasons])
            .WithDuration(command.Duration)
            .WithAmenities([.. command.Amenities])
            .WithMetadata(m =>
            {
                foreach (var entry in command.MetadataEntries)
                    m.AddEntry(entry.Label, entry.Value);
            });

        if (command.IsOutdoor) builder.Outdoor();
        if (command.IsFree) builder.Free();

        var attraction = builder.Build();
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

    public async Task AddChildAsync(AttractionId parentId, AttractionId childId)
    {
        var parent = await GetOrThrowAsync(parentId);
        var child = await GetOrThrowAsync(childId);
        parent.AddChild(child);
        await repository.SaveAsync(parent);
        await repository.SaveAsync(child);
    }

    public Task<Attraction?> GetByIdAsync(AttractionId id) => repository.GetByIdAsync(id);

    public Task<IReadOnlyList<Attraction>> GetAllAsync() => repository.GetAllAsync();

    public async Task<IReadOnlyList<Attraction>> FindBySpecAsync(ISpecification<Attraction> spec)
    {
        var all = await repository.GetAllAsync();
        return all.Where(a => spec.IsSatisfiedBy(a)).ToList().AsReadOnly();
    }

    private async Task<Attraction> GetOrThrowAsync(AttractionId id)
    {
        var attraction = await repository.GetByIdAsync(id);
        if (attraction is null)
            throw new EntityNotFoundException($"Attraction {id.Value} not found.");
        return attraction;
    }
}
