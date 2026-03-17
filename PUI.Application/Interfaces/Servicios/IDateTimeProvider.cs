namespace PUI.Application.Interfaces.Servicios
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        DateTime NowLocal { get; }

        DateTime ConvertToLocal(DateTime utcDate);
    }
}