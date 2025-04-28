namespace ShishaBuilder.Core.DTOs.TableDtos;
public class EditTable
{
    public int TableNumber { get; set; }

    public required bool IsBusy { get; set; }
}