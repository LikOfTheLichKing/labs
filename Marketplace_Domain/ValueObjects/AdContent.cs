using Marketplace.Domain.Exceptions;

namespace Marketplace.Domain.ValueObjects;
public record AdContent
{
    public string Title { get; }
    public string Description { get; }
    public IReadOnlyList<string> ImageUrls { get; }

    public AdContent(string title, string description, List<string>? imageUrls = null)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < 3)
            throw new DomainException("Заголовок оголошення повинен містити як мінімум 3 символи.");

        if (string.IsNullOrWhiteSpace(description) || description.Length < 10)
            throw new DomainException("Опис оголошення повинен містити як мінімум 10 символів.");

        Title = title.Trim();
        Description = description.Trim();
        ImageUrls = imageUrls?.AsReadOnly() ?? new List<string>().AsReadOnly();
    }
}