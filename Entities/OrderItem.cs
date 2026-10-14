namespace BackEnd.Entities;

// Linha do pedido. Só o Order cria itens (Create é internal).
public class OrderItem
{
    public const int QuantityMax = 99;

    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; } // cópia do preço no momento da compra

    public decimal Subtotal => UnitPrice * Quantity; // calculado, não vai pro banco

    private OrderItem() { }

    internal static OrderItem Create(Product product, int quantity) => new()
    {
        ProductId = ValidateProduct(product).Id,
        Quantity = ValidateQuantity(quantity),
        UnitPrice = product.Price, // snapshot: mudar o cardápio não altera pedidos antigos
    };

    private static Product ValidateProduct(Product? product)
    {
        // Id 0 = produto que nunca foi salvo no banco.
        if (product is null || product.Id <= 0)
            throw new DomainException("Produto do item inválido.");

        return product;
    }

    private static int ValidateQuantity(int quantity)
    {
        if (quantity < 1 || quantity > QuantityMax)
            throw new DomainException($"Quantidade deve estar entre 1 e {QuantityMax}.");

        return quantity;
    }
}
