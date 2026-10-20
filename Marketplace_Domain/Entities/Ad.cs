using Marketplace.Domain.Enums;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.ValueObjects;

namespace Marketplace.Domain.Entities;

public class Ad
{
    public Guid Id { get; private set; }
    public Guid Title { get; private set; }
    public Guid SellerId { get; private set; }
    public AdContent Content { get; private set; }
    public Money Price { get; private set; }
    public AdStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Ad(Guid id, Guid sellerId, AdContent content, Money price)
    {
        if (id == Guid.Empty)
            throw new DomainException("Ідентифікатор оголошення не може бути порожнім.");

        if (sellerId == Guid.Empty)
            throw new DomainException("Ідентифікатор продавця є обов'язковим.");

        Id = id;
        SellerId = sellerId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Status = AdStatus.Draft;
        CreatedAt = DateTime.UtcNow;
    }

    public void SubmitForModeration()
    {
        if (Status != AdStatus.Draft && Status != AdStatus.Rejected)
            throw new DomainException($"Неможливо відправити на модерацію оголошення зі статусом '{Status}'.");

        Status = AdStatus.PendingModeration;
        RejectionReason = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        if (Status != AdStatus.PendingModeration)
            throw new DomainException("Лише оголошення, що перебувають на модерації, можуть бути опубліковані.");

        Status = AdStatus.Published;
        RejectionReason = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject(string reason)
    {
        if (Status != AdStatus.PendingModeration)
            throw new DomainException("Лише оголошення на модерації можуть бути відхилені.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Причина відхилення обов'язкова.");

        Status = AdStatus.Rejected;
        RejectionReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        if (Status == AdStatus.Archived)
            throw new DomainException("Оголошення вже знаходиться в архіві.");

        Status = AdStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }
}