namespace WebApi.Models
{
    public class PlateDto
    {
        public Guid Id { get; set; }
        public required string LicenseText { get; set; }
        public bool IsActive { get; set; }
        public required bool IsLoading { get; set; }
        public required DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan WorkedTime { get; set; }
    }
}
