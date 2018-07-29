namespace DTO
{
    interface IErrorable<T>
    {
        bool HasError { get; set; }
        string Message { get; set; }
        T Data { get; set; }
    }
}