namespace BackEnd.Entities;

// Erro de regra de negócio: dado inválido ao criar/alterar uma entidade.
public class DomainException(string message) : Exception(message);
