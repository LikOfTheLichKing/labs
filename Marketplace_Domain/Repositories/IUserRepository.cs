using Marketplace.Domain.Entities;

namespace Marketplace.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}