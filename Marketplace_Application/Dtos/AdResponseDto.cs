namespace Marketplace.Application.Dtos;

public record AdResponseDto(
    Guid Id,
    Guid SellerId,
    string Title,
    string Description,
    decimal Price,
    string Currency,
    string Status,
    string? RejectionReason,
    DateTime CreatedAt
);