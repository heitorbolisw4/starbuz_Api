using System.Text.RegularExpressions;

namespace BackEnd.Entities;

public partial class Product
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 255;
    public const decimal PriceMax = 9_999.99m;

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    private Product() { }

    public static Product Create(string name, string description, decimal price) => new()
    {
        Name = NormalizeName(name),
        Description = NormalizeDescription(description),
        Price = ValidatePrice(price),
    };

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do produto é obrigatório.");

        var normalized = MultipleSpaces().Replace(name.Trim(), " ");

        if (normalized.Length > NameMaxLength)
            throw new DomainException($"Nome do produto deve ter no máximo {NameMaxLength} caracteres.");

        return normalized;
    }

    // Descrição é opcional: null/vazio vira "".
    private static string NormalizeDescription(string? description)
    {
        var normalized = MultipleSpaces().Replace(description?.Trim() ?? "", " ");

        if (normalized.Length > DescriptionMaxLength)
            throw new DomainException($"Descrição do produto deve ter no máximo {DescriptionMaxLength} caracteres.");

        return normalized;
    }

    // Dinheiro não é arredondado em silêncio: 12.999 é erro, não 13.00.
    private static decimal ValidatePrice(decimal price)
    {
        if (price <= 0 || price > PriceMax)
            throw new DomainException($"Preço do produto deve estar entre 0,01 e {PriceMax:N2}.");

        if (decimal.Round(price, 2) != price)
            throw new DomainException("Preço do produto deve ter no máximo 2 casas decimais.");

        return price;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpaces();
}
