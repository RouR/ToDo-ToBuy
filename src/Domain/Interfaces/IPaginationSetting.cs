namespace Domain.Interfaces
{
    public interface IPaginationSetting
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}