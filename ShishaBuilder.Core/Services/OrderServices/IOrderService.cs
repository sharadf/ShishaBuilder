using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.OrderRepositories;

namespace ShishaBuilder.Core.Services.OrderServices;

public interface IOrderService
{
    Task<Order> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);

}
