using Marketplace.Domain.Entities;

namespace Marketplace.Domain.Repositories;

public interface IAdRepository
{
    Task<Ad?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Ad ad, CancellationToken cancellationToken = default);
    Task UpdateAsync(Ad ad, CancellationToken cancellationToken = default);
}