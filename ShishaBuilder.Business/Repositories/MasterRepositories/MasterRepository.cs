using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.MasterRepositories;

namespace ShishaBuilder.Business.Repositories.MasterRepositories;

public class MasterRepository : IMasterRepository
{
    private AppDbContext context;

    public MasterRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddMasterAsync(Master master)
    {
        await context.Masters.AddAsync(master);
        await context.SaveChangesAsync();
    }

    public Task<Master?> FindByAppUserId(string appUserId)
    {
        return  context.Masters
        .Include(m => m.AppUser)
        .FirstOrDefaultAsync(m => m.AppUserId == appUserId && m.IsActive); 
    }

    public async Task<IEnumerable<Master>> GetAllMastersAsync()
    {
        // return await context
        //     .Masters.Include(m => m.AppUser) // Обязательно!
        //     .Where(m => m.IsActive)
        //     .ToListAsync();
        return await context.Masters.Include(m => m.AppUser).ToListAsync();
    }

    public async Task<Master?> GetMasterByIdAsync(int id)
    {
        return await context.Masters
        .Include(m=>m.AppUser)
        .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task UpdateMasterAsync(Master master)
    {
        context.Masters.Update(master);
        await context.SaveChangesAsync();
    }
   

}
