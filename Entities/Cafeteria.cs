using System.Text.RegularExpressions;

namespace BackEnd.Entities;

public partial class Cafeteria
{
    public const int CityMaxLength = 100;
    public const int AddressMaxLength = 255;

    public int Id { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; } // preenchido pelo banco (now())

    // N cafeterias -> 1 manager
    public int ManagerId { get; private set; }
    public Manager Manager { get; private set; } = null!;

    // 1:1 — pode ser null logo após criar a cafeteria, antes de cadastrar o admin.
    public Admin? Admin { get; private set; }

    private Cafeteria() { }

    public static Cafeteria Create(string city, string address, int managerId) => new()
    {
        City = NormalizeCity(city),
        Address = NormalizeAddress(address),
        ManagerId = ValidateManagerId(managerId),
    };

    private static string NormalizeCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("Cidade é obrigatória.");

        var normalized = MultipleSpaces().Replace(city.Trim(), " ");

        if (normalized.Length > CityMaxLength)
            throw new DomainException($"Cidade deve ter no máximo {CityMaxLength} caracteres.");

        return normalized;
    }

    private static string NormalizeAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Endereço é obrigatório.");

        var normalized = MultipleSpaces().Replace(address.Trim(), " ");

        if (normalized.Length > AddressMaxLength)
            throw new DomainException($"Endereço deve ter no máximo {AddressMaxLength} caracteres.");

        return normalized;
    }

    private static int ValidateManagerId(int managerId)
    {
        if (managerId <= 0)
            throw new DomainException("Manager da cafeteria inválido.");

        return managerId;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpaces();
}
