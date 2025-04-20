using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Services.TobaccoServices;

public interface ITobaccoService
{
    Task <IEnumerable<Tobacco>> GetAllTobaccosAsync();
    Task <Tobacco?> GetTobaccoByIdAsync(int id);
    Task UpdateTobaccoAsync(Tobacco tobacco);
    Task AddTobaccoAsync(Tobacco tobacco);
    Task SoftDeleteTobaccoAsync(int id);
    Task<IEnumerable<Tobacco>> GetAllDeletedTobaccosAsync();
}
