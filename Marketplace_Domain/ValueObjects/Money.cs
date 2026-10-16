using Marketplace.Domain.Exceptions;

namespace Marketplace.Domain.ValueObjects;
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "UAH")
    {
        if (amount < 0)
            throw new DomainException("Ціна не може бути від'ємною.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Валюта має бути вказана.");

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }
}