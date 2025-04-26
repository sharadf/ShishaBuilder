namespace ShishaBuilder.Core.Models;

public class Table
{
    public int Id { get; set; }

    public required int TableNumber { get; set; }

    public bool IsDeleted { get; set; } = false;

    public bool IsBusy { get; set; } = false;
}
