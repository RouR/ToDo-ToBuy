namespace Domain.Interfaces
{
    public interface ISortable
    {
        string OrderBy { get; set; }
        bool Asc { get; set; }
    }
}