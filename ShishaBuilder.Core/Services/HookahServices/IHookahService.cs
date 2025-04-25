using ShishaBuilder.Core.Models;

public interface IHookahService
{
    Task<IEnumerable<Hookah>> GetAllHookahsAsync();
    Task<Hookah?> GetByIdHookahAsync(int id);
    Task AddHookahAsync(Hookah hookah);
    Task UpdateHookahAsync(Hookah hookah);
    Task SoftDeleteHookahAsync(int id);
    Task<IEnumerable<Hookah>> GetAllDeletedHookahsAsync();
}