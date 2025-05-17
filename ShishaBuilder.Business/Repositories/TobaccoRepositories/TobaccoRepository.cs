using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories;

namespace ShishaBuilder.Business.Repositories;

public class TobaccoRepository : ITobaccoRepository
{
    private AppDbContext context;

    public TobaccoRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddTobaccoAsync(Tobacco tobacco)
    {
        await context.Tobaccos.AddAsync(tobacco);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tobacco>> GetAllTobaccosAsync()
    {
        return await context.Tobaccos.ToListAsync();
    }

    public async Task<Tobacco?> GetTobaccoByIdAsync(int id)
    {
        return await context.Tobaccos.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task UpdateTobaccoAsync(Tobacco tobacco)
    {
        context.Tobaccos.Update(tobacco);
        await context.SaveChangesAsync();
    }
}
