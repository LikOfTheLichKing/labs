using System.Collections.Concurrent;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Repositories;

namespace Marketplace.Infrastructure.Persistence;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _storage = new();

    public InMemoryUserRepository()
    {
        var testUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        _storage[testUserId] = new User(testUserId, "Ярослав Кіхая", "yaroslav@example.com");
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }
}