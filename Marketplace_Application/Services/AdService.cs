using Marketplace.Application.Dtos;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Repositories;
using Marketplace.Domain.ValueObjects;

namespace Marketplace.Application.Services;

public class AdService : IAdService
{
    private readonly IAdRepository _adRepository;
    private readonly IUserRepository _userRepository;

    public AdService(IAdRepository adRepository, IUserRepository userRepository)
    {
        _adRepository = adRepository;
        _userRepository = userRepository;
    }

    public async Task<AdResponseDto> CreateAdAsync(CreateAdDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(dto.SellerId, cancellationToken);
        if (user == null)
            throw new DomainException("Продавця з таким ID не знайдено.");

        var content = new AdContent(dto.Title, dto.Description, dto.ImageUrls);
        var price = new Money(dto.Price, dto.Currency);
        
        var ad = new Ad(Guid.NewGuid(), dto.SellerId, content, price);

        await _adRepository.AddAsync(ad, cancellationToken);

        return MapToDto(ad);
    }

    public async Task<AdResponseDto> SubmitForModerationAsync(Guid adId, CancellationToken cancellationToken = default)
    {
        var ad = await _adRepository.GetByIdAsync(adId, cancellationToken);
        if (ad == null)
            throw new DomainException("Оголошення не знайдено.");

        ad.SubmitForModeration();

        await _adRepository.UpdateAsync(ad, cancellationToken);

        return MapToDto(ad);
    }

    public async Task<AdResponseDto> ModerateAdAsync(Guid adId, bool isApproved, string? rejectionReason = null, CancellationToken cancellationToken = default)
    {
        var ad = await _adRepository.GetByIdAsync(adId, cancellationToken);
        if (ad == null)
            throw new DomainException("Оголошення не знайдено.");

        if (isApproved)
        {
            ad.Approve();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(rejectionReason))
                throw new DomainException("Вкажіть причину відхилення.");

            ad.Reject(rejectionReason);
        }

        await _adRepository.UpdateAsync(ad, cancellationToken);

        return MapToDto(ad);
    }

    public async Task<AdResponseDto> GetAdByIdAsync(Guid adId, CancellationToken cancellationToken = default)
    {
        var ad = await _adRepository.GetByIdAsync(adId, cancellationToken);
        if (ad == null)
            throw new DomainException("Оголошення не знайдено.");

        return MapToDto(ad);
    }

    private static AdResponseDto MapToDto(Ad ad)
    {
        return new AdResponseDto(
            ad.Id,
            ad.SellerId,
            ad.Content.Title,
            ad.Content.Description,
            ad.Price.Amount,
            ad.Price.Currency,
            ad.Status.ToString(),
            ad.RejectionReason,
            ad.CreatedAt
        );
    }
}