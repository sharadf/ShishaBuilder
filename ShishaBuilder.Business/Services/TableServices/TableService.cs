using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.TableRepositories;
using ShishaBuilder.Core.Services.TableServices;

namespace ShishaBuilder.Business.Services.HookahServices;

public class TableService : ITableService
{
    private readonly ITableRepository tableRepository;

    public TableService(ITableRepository tableRepository)
    {
        this.tableRepository = tableRepository;
    }

    public async Task<IEnumerable<Table>> GetAllTablesAsync()
    {
        var allTables = await tableRepository.GetAllTablesAsync();
        return allTables.Where(t => t.IsDeleted == false);
    }

    public async Task<Table?> GetByIdTableAsync(int id)
    {
        return await tableRepository.GetByIdTableAsync(id);
    }

    public async Task AddTableAsync(Table createTable)
    {
        if (createTable == null)
            throw new ArgumentNullException(nameof(createTable), "Table can not be null");

        await tableRepository.CreateTableAsync(createTable);
    }

    public async Task UpdateTableAsync(Table editTable)
    {
        await tableRepository.UpdateTableAsync(editTable);
    }

    public async Task SoftDeleteTableAsync(int id)
    {
        var tableDel = await tableRepository.GetByIdTableAsync(id);
        if (tableDel  != null)
        {
            tableDel.IsDeleted = true;
            await tableRepository.UpdateTableAsync(tableDel);
        }
    }

    public async Task<IEnumerable<Table>> GetAllDeletedTablesAsync()
    {
        var all = await tableRepository.GetAllTablesAsync();

        return all.Where(t => t.IsDeleted == true);
    }

    public async Task<Table> GetByTableNumber(int tableNumber)
    {
        var table =await tableRepository.GetByTableNumber(tableNumber);
        return table;
    }
}
