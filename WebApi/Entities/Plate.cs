using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Entities
{
    public class Plate
    {
        [Key]
        public Guid Id {  get; set; }

        [Required]
        [MaxLength(50)]
        public required string LicenseText { get; set; }

        [Required]
        [MaxLength(200)]
        public required DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public TimeSpan WorkedTime { get; set; }

        public bool IsActive { get; set; }
        public bool IsLoading { get; set; }

        public Plate()
        {
        }

        [SetsRequiredMembers]
        public Plate(Guid id, string licenseText, DateTime startTime, DateTime endTime, bool isActive, bool isLoading)
        {
            Id = id;
            LicenseText = licenseText;
            StartTime = startTime;
            EndTime = endTime;
            IsActive = isActive;
            IsLoading = isLoading;
        }
    }
}
