namespace Marketplace.Application.Dtos;

public record CreateAdDto(
    Guid SellerId,
    string Title,
    string Description,
    decimal Price,
    string Currency = "UAH",
    List<string>? ImageUrls = null
);