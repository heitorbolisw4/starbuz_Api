namespace BackEnd.Entities;

public class Cafeteria
{
    public int Id { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public DateTimeOffset CreatedAt { get; set; } // preenchido pelo banco (now())

    // N cafeterias -> 1 manager
    public int ManagerId { get; set; }
    public Manager Manager { get; set; } = null!;

    // 1:1 — pode ser null logo após criar a cafeteria, antes de cadastrar o admin.
    public Admin? Admin { get; set; }
}
