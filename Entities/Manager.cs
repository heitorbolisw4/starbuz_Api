namespace BackEnd.Entities;

// 1 manager -> N cafeterias
public class Manager
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; } // preenchido pelo banco (now())

    public List<Cafeteria> Cafeterias { get; } = [];
}
