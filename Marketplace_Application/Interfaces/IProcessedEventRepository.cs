namespace Marketplace.Domain.Repositories;

public interface IProcessedEventRepository
{
    Task<bool> HasBeenProcessedAsync(Guid eventId, CancellationToken ct = default);
    Task MarkAsProcessedAsync(Guid eventId, CancellationToken ct = default);
}