namespace WebApi.Models
{
    public class PlateForUpdateDto
    {
        public string? LicenseText { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoading { get; set; }
    }
}
