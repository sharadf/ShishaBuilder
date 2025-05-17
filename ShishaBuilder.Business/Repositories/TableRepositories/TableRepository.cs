using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.TableRepositories;

namespace ShishaBuilder.Business.Repositories.TableRepositories;

public class TableRepository : ITableRepository
{
    private readonly AppDbContext context;

    public TableRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<ShishaBuilder.Core.Models.Table>> GetAllTablesAsync()
    {
        return await context.Tables.ToListAsync();
    }

    public async Task<ShishaBuilder.Core.Models.Table?> GetByIdTableAsync(int id)
    {
        return await context.Tables.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task CreateTableAsync(ShishaBuilder.Core.Models.Table createTable)
    {
        bool exists = await context.Tables.AnyAsync(t =>
            t.TableNumber == createTable.TableNumber && !t.IsDeleted
        );

        if (exists)
            throw new Exception("Table with this number already exists.");

        context.Tables.Add(createTable);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTableAsync(ShishaBuilder.Core.Models.Table editTable)
    {
        context.Tables.Update(editTable);
        await context.SaveChangesAsync();
    }

    public async Task<Core.Models.Table> GetByTableNumber(int tableNumber)
    {
        return await context.Tables.FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
    }
}
