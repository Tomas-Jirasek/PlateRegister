using WebApi.Entities;
using WebApi.Interfaces;
namespace WebApi.Extensions
{
    public static class DateTimeExtensions
    {
        private static TimeZoneInfo localTimeZone = Environment.OSVersion.Platform == PlatformID.Win32NT ? TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time") : TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        public static IEnumerable<T> ConvertEnumerableUtcToLocal<T>(this IEnumerable<T> items) where T : IHasUtcStartEndDateTime
        {
                foreach (var item in items)
                {
                    item.StartTime = TimeZoneInfo.ConvertTimeFromUtc(item.StartTime, localTimeZone);
                    item.EndTime = TimeZoneInfo.ConvertTimeFromUtc(item.EndTime, localTimeZone);
                }

                return items;
        }

        public static IHasUtcStartEndDateTime ConvertItemUtcToLocal(this IHasUtcStartEndDateTime item)
        {

            item.StartTime = TimeZoneInfo.ConvertTimeFromUtc(item.StartTime, localTimeZone);
            item.EndTime = TimeZoneInfo.ConvertTimeFromUtc(item.EndTime, localTimeZone);

            return item;
        }
    }
}
