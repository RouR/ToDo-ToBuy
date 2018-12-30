namespace Domain.Interfaces
{
    public interface IPaginationSetting
    {
        int Page { get; }
        int PageSize { get; }
    }
}