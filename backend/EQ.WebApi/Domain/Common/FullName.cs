namespace EQ.WebApi.Domain.Common;

public sealed record FullName(string Name, string Surname, string? Patronymic)
{
    public string GetFullName()
    {
        if (string.IsNullOrWhiteSpace(Patronymic))
            return $"{Surname} {Name}";

        return $"{Surname} {Name} {Patronymic}";
    }
}