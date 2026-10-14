using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.UnitPrice).HasPrecision(6, 2);
        builder.Ignore(i => i.Subtotal);

        // Um produto aparece no máximo 1x por pedido.
        builder.HasIndex(i => new { i.OrderId, i.ProductId }).IsUnique();

        // Ref: OrderItem.ProductId > Product.Id [delete: restrict]
        // Restrict: produto com venda não pode ser apagado (histórico).
        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
