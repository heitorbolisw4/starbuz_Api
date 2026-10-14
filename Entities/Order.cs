namespace BackEnd.Entities;

// Carrinho finalizado. Depois de criado, não muda.
public class Order
{
    public int Id { get; private set; }
    public decimal Total { get; private set; } // gravado: é o valor que foi cobrado
    public DateTimeOffset CreatedAt { get; private set; } // preenchido pelo banco (now())

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items;

    private Order() { }

    // Recebe o carrinho do front: produto + quantidade.
    public static Order Create(IEnumerable<(Product Product, int Quantity)> cart)
    {
        var order = new Order();
        order._items.AddRange(BuildItems(cart));
        order.Total = order._items.Sum(i => i.Subtotal);
        return order;
    }

    private static List<OrderItem> BuildItems(IEnumerable<(Product Product, int Quantity)>? cart)
    {
        var lines = cart?.ToList() ?? [];

        if (lines.Count == 0)
            throw new DomainException("Pedido precisa ter pelo menos um produto.");

        // Mesmo produto 2x no carrinho vira 1 linha com a quantidade somada.
        return lines
            .GroupBy(l => l.Product?.Id)
            .Select(g => OrderItem.Create(g.First().Product, g.Sum(l => l.Quantity)))
            .ToList();
    }
}
