using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Repositories.MasterRepositories;

public interface IMasterRepository
{
    Task AddMasterAsync(Master master);
    Task <Master> GetMasterByIdAsync(int id);
    Task UpdateMasterAsync(Master master);
    Task<IEnumerable<Master>> GetAllMastersAsync();
}
