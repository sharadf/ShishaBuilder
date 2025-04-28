using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.OrderRepositories;
using ShishaBuilder.Core.Services.OrderServices;

namespace ShishaBuilder.Business.Services.OrderServices;

public class OrderService : IOrderService
{
    private IOrderRepository orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        this.orderRepository=orderRepository;
    }
    public async Task<Order> AddOrderAsync(Order order)
    {
        if (order==null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        }
        return await orderRepository.AddOrderAsync(order);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await orderRepository.GetAllOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await orderRepository.GetOrderByIdAsync(id);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        await orderRepository.UpdateOrderAsync(order);
    }
}
