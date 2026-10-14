using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BackEnd.Entities;

// 1:1 com Cafeteria. A FK fica aqui: admin não existe sem cafeteria.
public partial class Admin
{
    public const int NameMaxLength = 50;
    public const int EmailMaxLength = 254;

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public int CafeteriaId { get; private set; }
    public Cafeteria Cafeteria { get; private set; } = null!;

    // Usado pelo EF e pelo Create. Ninguém de fora instancia direto.
    private Admin() { }

    public static Admin Create(string name, string email, int cafeteriaId) => new()
    {
        Name = NormalizeName(name),
        Email = NormalizeEmail(email),
        CafeteriaId = ValidateCafeteriaId(cafeteriaId),
    };

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do admin é obrigatório.");

        var normalized = MultipleSpaces().Replace(name.Trim(), " ");

        if (normalized.Length > NameMaxLength)
            throw new DomainException($"Nome do admin deve ter no máximo {NameMaxLength} caracteres.");

        return normalized;
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email do admin é obrigatório.");

        var normalized = email.Trim().ToLowerInvariant();

        if (normalized.Length > EmailMaxLength)
            throw new DomainException($"Email do admin deve ter no máximo {EmailMaxLength} caracteres.");

        // Address == normalized barra formatos como "Fulano <a@b.com>".
        if (!MailAddress.TryCreate(normalized, out var parsed) || parsed.Address != normalized)
            throw new DomainException("Email do admin inválido.");

        return normalized;
    }

    private static int ValidateCafeteriaId(int cafeteriaId)
    {
        if (cafeteriaId <= 0)
            throw new DomainException("Cafeteria do admin inválida.");

        return cafeteriaId;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpaces();
}
