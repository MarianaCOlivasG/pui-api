using PUI.Application.Interfaces.Servicios;


namespace PUI.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly TimeZoneInfo _zona;

        public DateTimeProvider()
        {
            _zona = TimeZoneInfo.FindSystemTimeZoneById("America/Cancun");
        }

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime NowLocal =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _zona);

        public DateTime ConvertToLocal(DateTime utcDate)
        {
            if (utcDate.Kind == DateTimeKind.Unspecified)
                utcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, _zona);
        }
    }
}
