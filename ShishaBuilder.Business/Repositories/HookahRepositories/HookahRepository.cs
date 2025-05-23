using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.HookahRepositories;

namespace ShishaBuilder.Business.Repositories.HookahRepositories;

public class HookahRepository : IHookahRepository
{
    private readonly AppDbContext context;

    public HookahRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Hookah>> GetAllHookahsAsync()
    {
        return await context.Hookahs.ToListAsync();
    }

    public async Task<Hookah?> GetByIdHookahAsync(int id)
    {
        return await context.Hookahs.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task CreateHookahAsync(Hookah createHookah)
    {
        context.Hookahs.Add(createHookah);
        await context.SaveChangesAsync();
    }

    public async Task UpdateHookahAsync(Hookah editHokah)
    {
        context.Hookahs.Update(editHokah);
        await context.SaveChangesAsync();
    }
}