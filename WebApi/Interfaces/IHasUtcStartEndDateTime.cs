namespace WebApi.Interfaces
{
    public interface IHasUtcStartEndDateTime
    {
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
    }
}
