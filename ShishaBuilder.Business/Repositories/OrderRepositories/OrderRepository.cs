using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.OrderRepositories;

namespace ShishaBuilder.Business.Repositories.OrderRepositories;

public class OrderRepository : IOrderRepository
{
    private AppDbContext context;

    public OrderRepository(AppDbContext context)
    {
        this.context=context;        
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // 1. Сохраняем Order (чтобы получить ID)
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            
            // 2. Устанавливаем OrderId для всех OrderTobacco и сохраняем их
            foreach (var tobacco in order.OrderTobaccos)
            {
                tobacco.OrderId = order.Id; // Устанавливаем связь
                await context.OrderTobaccos.AddAsync(tobacco);
            }
            
            await context.SaveChangesAsync(); // Сохраняем табаки
            await transaction.CommitAsync();
            
            return order;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error saving order: {ex.Message}", ex);
        }
    }
  public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await context.Orders
            .Include(o => o.OrderTobaccos) // Добавляем загрузку табаков
            .ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.OrderTobaccos) 
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        // Загружаем существующий заказ с табаками
        var existingOrder = await context.Orders
            .Include(o => o.OrderTobaccos)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (existingOrder == null)
            throw new Exception("Order not found");

        // Обновляем основные поля
        context.Entry(existingOrder).CurrentValues.SetValues(order);

        // Обновляем коллекцию табаков
        UpdateOrderTobaccos(existingOrder, order.OrderTobaccos);

        await context.SaveChangesAsync();
    }
    private void UpdateOrderTobaccos(Order existingOrder, ICollection<OrderTobacco> newTobaccos)
    {
        // Удаляем отсутствующие
        foreach (var existing in existingOrder.OrderTobaccos.ToList())
        {
            if (!newTobaccos.Any(ot => ot.TobaccoId == existing.TobaccoId))
                context.OrderTobaccos.Remove(existing);
        }

        // Добавляем/обновляем
        foreach (var newTobacco in newTobaccos)
        {
            var existing = existingOrder.OrderTobaccos
                .FirstOrDefault(ot => ot.TobaccoId == newTobacco.TobaccoId);
            
            if (existing != null)
            {
                // Обновляем существующий
                context.Entry(existing).CurrentValues.SetValues(newTobacco);
            }
            else
            {
                // Добавляем новый
                existingOrder.OrderTobaccos.Add(new OrderTobacco
                {
                    TobaccoId = newTobacco.TobaccoId,
                    Percentage = newTobacco.Percentage
                });
            }
        }
    }
}
