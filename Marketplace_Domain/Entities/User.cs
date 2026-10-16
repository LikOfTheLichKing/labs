using Marketplace.Domain.Exceptions;

namespace Marketplace.Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public User(Guid id, string name, string email)
    {
        if (id == Guid.Empty)
            throw new DomainException("Ідентифікатор користувача не може бути порожнім.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Ім'я користувача є обов'язковим.");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("Некоректний Email користувача.");

        Id = id;
        Name = name;
        Email = email;
    }
}