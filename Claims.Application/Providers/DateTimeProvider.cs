using Claims.Application.Interfaces;

namespace Claims.Application.Providers
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime DateTimeNow() => DateTime.Now;
    }
}
