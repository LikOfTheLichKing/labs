using Marketplace.Application.Dtos;

namespace Marketplace.Application.Interfaces;

public interface IAdService
{
    Task<AdResponseDto> CreateAdAsync(CreateAdDto dto, CancellationToken cancellationToken = default);
    Task<AdResponseDto> SubmitForModerationAsync(Guid adId, CancellationToken cancellationToken = default);
    Task<AdResponseDto> ModerateAdAsync(Guid adId, bool isApproved, string? rejectionReason = null, CancellationToken cancellationToken = default);
    Task<AdResponseDto> GetAdByIdAsync(Guid adId, CancellationToken cancellationToken = default);
}