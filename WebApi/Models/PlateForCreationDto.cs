namespace WebApi.Models
{
    public class PlateForCreationDto
    {
        public required string LicenseText { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } = null;
        public bool IsActive { get; set; }
        public required bool IsLoading { get; set; }
    }
}
