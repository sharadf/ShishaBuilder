using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Services.MasterServices;

public interface IMasterService
{

    
    Task AddMasterAsync(Master master);
    Task <Master> GetMasterByIdAsync(int id);
    Task UpdateMasterAsync(Master master);
    Task<IEnumerable<Master>> GetAllMastersAsync();
    Task SoftDeleteMasterAsync (int id );
    Task<IEnumerable<Master>> GetAllDeletedMastersAsync();

    
}
