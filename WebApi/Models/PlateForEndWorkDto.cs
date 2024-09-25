namespace WebApi.Models
{
    public class PlateForEndWorkDto
    {
        public required bool IsActive { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan WorkedTime { get; set; }
    }
}
