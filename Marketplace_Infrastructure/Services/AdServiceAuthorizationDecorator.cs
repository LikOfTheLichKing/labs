using Marketplace.Application.Dtos;
using Marketplace.Application.Interfaces;

namespace Marketplace.Application.Services;

public class AdServiceAuthorizationDecorator : IAdService
{
    private readonly IAdService _innerService;
    private readonly ICurrentUserService _currentUserService;

    public AdServiceAuthorizationDecorator(IAdService innerService, ICurrentUserService currentUserService)
    {
        _innerService = innerService;
        _currentUserService = currentUserService;
    }

    public async Task<AdResponseDto> CreateAdAsync(CreateAdDto dto, CancellationToken ct = default)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Для создания объявления необходима аутентификация.");
        }

        return await _innerService.CreateAdAsync(dto, ct);
    }

    public async Task<AdResponseDto> SubmitForModerationAsync(Guid id, CancellationToken ct = default)
    {
        var existingAd = await _innerService.GetAdByIdAsync(id, ct);
        if (existingAd == null)
        {
            throw new KeyNotFoundException($"Объявление с ID {id} не найдено.");
        }

        // Авторизация на уровне владельца ресурса: только владелец может отправить на модерацию
        if (existingAd.SellerId != _currentUserService.UserId && _currentUserService.Role != "Admin")
        {
            throw new UnauthorizedAccessException("У вас нет прав на редактирование этого объявления.");
        }

        return await _innerService.SubmitForModerationAsync(id, ct);
    }

    public async Task<AdResponseDto> ModerateAdAsync(Guid id, bool isApproved, string? reason, CancellationToken ct = default)
    {
        // Авторизация по роли: модерацию может проводить только пользователь с ролью Moderator или Admin
        if (_currentUserService.Role != "Moderator" && _currentUserService.Role != "Admin")
        {
            throw new UnauthorizedAccessException("Доступ запрещен. Требуется роль Moderator.");
        }

        return await _innerService.ModerateAdAsync(id, isApproved, reason, ct);
    }

    public async Task<AdResponseDto?> GetAdByIdAsync(Guid id, CancellationToken ct = default)
    {
        // Чтение каталога доступно всем пользователям
        return await _innerService.GetAdByIdAsync(id, ct);
    }
}