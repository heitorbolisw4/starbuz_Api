namespace BackEnd.Entities;

// 1:1 com Cafeteria. A FK fica aqui: admin não existe sem cafeteria.
public class Admin
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public int CafeteriaId { get; set; }
    public Cafeteria Cafeteria { get; set; } = null!;
}
