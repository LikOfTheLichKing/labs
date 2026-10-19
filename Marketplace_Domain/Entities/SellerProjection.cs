namespace Marketplace.Domain.Entities;

public class SellerProjection
{
    public Guid UserId { get; private set; }
    public string FullName { get; private set; }
    public bool IsBlocked { get; private set; }

    public SellerProjection(Guid userId, string fullName, bool isBlocked)
    {
        UserId = userId;
        FullName = fullName;
        IsBlocked = isBlocked;
    }

    public void UpdateInfo(string fullName, bool isBlocked)
    {
        FullName = fullName;
        IsBlocked = isBlocked;
    }
}