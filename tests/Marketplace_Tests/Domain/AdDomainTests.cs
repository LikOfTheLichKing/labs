using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.ValueObjects;
using Xunit;

namespace Marketplace.Tests.Domain;

public class AdDomainTests
{
    private readonly Guid _sellerId = Guid.NewGuid();
    private readonly AdContent _validContent = new("Продаж ноутбука", "Хороший стан, використовувався 1 рік.");
    private readonly Money _validPrice = new(15000, "UAH");

    [Fact]
    public void CreateAd_ShouldSetStatusToDraft()
    {
        var ad = new Ad(Guid.NewGuid(), _sellerId, _validContent, _validPrice);

        Assert.Equal(AdStatus.Draft, ad.Status);
    }

    [Fact]
    public void SubmitForModeration_FromDraft_ShouldChangeStatusToPendingModeration()
    {
        var ad = new Ad(Guid.NewGuid(), _sellerId, _validContent, _validPrice);

        ad.SubmitForModeration();

        Assert.Equal(AdStatus.PendingModeration, ad.Status);
    }

    [Fact]
    public void Approve_WhenPendingModeration_ShouldChangeStatusToPublished()
    {
        var ad = new Ad(Guid.NewGuid(), _sellerId, _validContent, _validPrice);
        ad.SubmitForModeration();

        ad.Approve();

        Assert.Equal(AdStatus.Published, ad.Status);
    }

    [Fact]
    public void Approve_WhenDraft_ShouldThrowDomainException()
    {
        var ad = new Ad(Guid.NewGuid(), _sellerId, _validContent, _validPrice);

        Assert.Throws<DomainException>(() => ad.Approve());
    }

    [Fact]
    public void Reject_WithoutReason_ShouldThrowDomainException()
    {
        var ad = new Ad(Guid.NewGuid(), _sellerId, _validContent, _validPrice);
        ad.SubmitForModeration();

        Assert.Throws<DomainException>(() => ad.Reject(""));
    }
}