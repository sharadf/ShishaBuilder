using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Base;
public interface IHookahRepository
{
    Task<IEnumerable<Hookah>> GetAllHookahsAsync();
    Task<Hookah?> GetByIdHookahAsync(int id);
    Task CreateHookahAsync(Hookah hookah);
    Task UpdateHookahAsync(Hookah hookah);
}