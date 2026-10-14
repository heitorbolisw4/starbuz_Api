using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BackEnd.Entities;

// 1 manager -> N cafeterias
public partial class Manager
{
    public const int NameMaxLength = 50;
    public const int PhoneMaxLength = 20;
    public const int EmailMaxLength = 254;

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty; // só dígitos: DDD + número
    public string Email { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; } // preenchido pelo banco (now())

    private readonly List<Cafeteria> _cafeterias = [];
    public IReadOnlyCollection<Cafeteria> Cafeterias => _cafeterias;

    private Manager() { }

    public static Manager Create(string name, string phone, string email) => new()
    {
        Name = NormalizeName(name),
        Phone = NormalizePhone(phone),
        Email = NormalizeEmail(email),
    };

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do manager é obrigatório.");

        var normalized = MultipleSpaces().Replace(name.Trim(), " ");

        if (normalized.Length > NameMaxLength)
            throw new DomainException($"Nome do manager deve ter no máximo {NameMaxLength} caracteres.");

        return normalized;
    }

    // "(34) 99999-0000", "+55 34 99999-0000" -> "34999990000"
    private static string NormalizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Telefone do manager é obrigatório.");

        var digits = NonDigits().Replace(phone, "");

        // Remove o código do Brasil se vier junto.
        if (digits.Length is 12 or 13 && digits.StartsWith("55"))
            digits = digits[2..];

        // Fixo: DDD + 8 dígitos. Celular: DDD + 9 dígitos.
        if (digits.Length is not (10 or 11))
            throw new DomainException("Telefone do manager deve ter DDD + número (10 ou 11 dígitos).");

        return digits;
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email do manager é obrigatório.");

        var normalized = email.Trim().ToLowerInvariant();

        if (normalized.Length > EmailMaxLength)
            throw new DomainException($"Email do manager deve ter no máximo {EmailMaxLength} caracteres.");

        if (!MailAddress.TryCreate(normalized, out var parsed) || parsed.Address != normalized)
            throw new DomainException("Email do manager inválido.");

        return normalized;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpaces();

    [GeneratedRegex(@"\D")]
    private static partial Regex NonDigits();
}
