using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Marketplace.Application.Interfaces;

namespace Marketplace.Infrastructure.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            return Guid.TryParse(userIdClaim, out var id) ? id : null;
        }
    }

    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}