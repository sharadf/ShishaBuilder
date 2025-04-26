using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories;
using ShishaBuilder.Core.Services.TobaccoServices;

namespace ShishaBuilder.Business.Services.TobaccoServices;

public class TobaccoService : ITobaccoService
{
    private ITobaccoRepository tobaccoRepository;

    public TobaccoService(ITobaccoRepository tobaccoRepository)
    {
        this.tobaccoRepository = tobaccoRepository;
    }

    public async Task AddTobaccoAsync(Tobacco tobacco)
    {
        if (tobacco == null)
        {
            throw new ArgumentNullException(nameof(tobacco), "Tobacco cannot be null.");
        }
        await tobaccoRepository.AddTobaccoAsync(tobacco);
    }

    public async Task<IEnumerable<Tobacco>> GetAllTobaccosAsync()
    {
        var all = await tobaccoRepository.GetAllTobaccosAsync();

        return all.Where(t => t.IsDeleted == false);
    }

    public async Task<IEnumerable<Tobacco>> GetAllDeletedTobaccosAsync()
    {
        var all = await tobaccoRepository.GetAllTobaccosAsync();

        return all.Where(t => t.IsDeleted == true);
    }

    public async Task<Tobacco?> GetTobaccoByIdAsync(int id)
    {
        return await tobaccoRepository.GetTobaccoByIdAsync(id);
    }

    public async Task SoftDeleteTobaccoAsync(int id)
    {
        var tobacco = await tobaccoRepository.GetTobaccoByIdAsync(id);
        if (tobacco != null)
        {
            tobacco.IsDeleted = true;
            await tobaccoRepository.UpdateTobaccoAsync(tobacco);
        }
    }

    public async Task UpdateTobaccoAsync(Tobacco tobacco)
    {
        await tobaccoRepository.UpdateTobaccoAsync(tobacco);
    }
}
