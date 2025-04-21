using ShishaBuilder.Core.Base;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Business.Repositories;

public class HookahService : IHookahService
{
    private readonly IHookahRepository hookahRepository;
    public HookahService(IHookahRepository hookahRepository)
    {
        this.hookahRepository = hookahRepository;
    }

    public async Task<IEnumerable<Hookah>> GetAllHookahsAsync()
    {
        var allHookahs = await hookahRepository.GetAllHookahsAsync();
        return allHookahs.Where(t => t.IsDeleted == false);
    }

    public async Task<Hookah?> GetByIdHookahAsync(int id)
    {
        return await hookahRepository.GetByIdHookahAsync(id);
    }

    public async Task AddHookahAsync(Hookah createHookah)
    {
        if (createHookah == null)
            throw new ArgumentNullException(nameof(createHookah), "Hookah can not be null");

        await hookahRepository.CreateHookahAsync(createHookah);
    }


    public async Task UpdateHookahAsync(Hookah editHokah)
    {
        await hookahRepository.UpdateHookahAsync(editHokah);
    }


    public async Task SoftDeleteHookahAsync(int id)
    {
        var hookahDel = await hookahRepository.GetByIdHookahAsync(id);
        if (hookahDel != null)
        {
            hookahDel.IsDeleted = true;
            await hookahRepository.UpdateHookahAsync(hookahDel);
        }
    }


    public async Task<IEnumerable<Hookah>> GetAllDeletedHookahsAsync()
    {
        var all = await hookahRepository.GetAllHookahsAsync();

        return all
            .Where(t => t.IsDeleted == true);
    }
}