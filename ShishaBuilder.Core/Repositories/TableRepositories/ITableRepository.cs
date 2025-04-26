using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShishaBuilder.Core.Repositories.TableRepositories;

public interface ITableRepository
{
    Task<IEnumerable<ShishaBuilder.Core.Models.Table>> GetAllTablesAsync();
    Task<ShishaBuilder.Core.Models.Table?> GetByIdTableAsync(int id);
    Task CreateTableAsync(ShishaBuilder.Core.Models.Table table);
    Task UpdateTableAsync(ShishaBuilder.Core.Models.Table table);
}