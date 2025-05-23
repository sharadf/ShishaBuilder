using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.DTOs.OrderDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.OrderRepositories;

namespace ShishaBuilder.Core.Services.OrderServices;

public interface IOrderService
{
    Task<Order> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<int> GetTotalOrdersCountAsync();
    Task<Dictionary<int, int>> GetTobaccoUsageStatsAsync();
    Task<AllOrdersViewModelDto> GetOrdersViewModelAsync(int orderId);
    Task AssignOrderToMasterAsync(int orderId, int masterId);
    Task<List<AllOrdersViewModelDto>> GetOrdersByMasterAsync(int masterId);
    Task<bool> UpdateOrderStatusAsync(int orderId, int masterId, OrderStatus newStatus);
}
