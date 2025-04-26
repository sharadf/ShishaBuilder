using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShishaBuilder.Core.Services.TableServices;

public interface ITableService
{
    Task<IEnumerable<ShishaBuilder.Core.Models.Table>> GetAllTablesAsync();
    Task<ShishaBuilder.Core.Models.Table?> GetByIdTableAsync(int id);

    Task AddTableAsync(ShishaBuilder.Core.Models.Table table);
    Task UpdateTableAsync(ShishaBuilder.Core.Models.Table table);
    Task SoftDeleteTableAsync(int id);
    Task<IEnumerable<ShishaBuilder.Core.Models.Table>> GetAllDeletedTablesAsync();
}
