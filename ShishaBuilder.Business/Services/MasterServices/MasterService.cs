using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.MasterRepositories;
using ShishaBuilder.Core.Services.MasterServices;

namespace ShishaBuilder.Business.Services.MasterServices;

public class MasterService : IMasterService
{
    private IMasterRepository masterRepository;

    public MasterService(IMasterRepository masterRepository)
    {
        this.masterRepository = masterRepository;
    }

    public async Task AddMasterAsync(Master master)
    {
        if (master == null)
        {
            throw new ArgumentNullException();
        }

        await masterRepository.AddMasterAsync(master);
    }

    public async Task<IEnumerable<Master>> GetAllDeletedMastersAsync()
    {
        var allMasters = await masterRepository.GetAllMastersAsync();

        return allMasters.Where(t => t.IsActive == false);
    }

    public async Task<IEnumerable<Master>> GetAllMastersAsync()
    {
        var allMasters = await masterRepository.GetAllMastersAsync();

        return allMasters.Where(t => t.IsActive == true);
    }

    public async Task<Master> GetMasterByIdAsync(int id)
    {
        var master = await masterRepository.GetMasterByIdAsync(id);
        if (master == null)
        {
            throw new KeyNotFoundException($"Master with ID {id} was not found.");
        }
        return master;
    }

    public async Task SoftDeleteMasterAsync(int id)
    {
        var master = await masterRepository.GetMasterByIdAsync(id);
        if (master == null)
        {
            throw new KeyNotFoundException($"Master with ID {id} was not found.");
        }
        master.IsActive = false;
        await masterRepository.UpdateMasterAsync(master);
    }

    public async Task UpdateMasterAsync(Master master)
    {
        await masterRepository.UpdateMasterAsync(master);
    }
}
