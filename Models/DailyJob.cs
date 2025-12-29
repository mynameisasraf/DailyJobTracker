using System;
using System.ComponentModel.DataAnnotations;

namespace DailyJobTracker.Models
{
    public class DailyJob
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Work Date")]
        public DateTime WorkDate { get; set; }

        [Required]
        public string Shift { get; set; } = "Morning";

        [Required]
        [Display(Name = "Engineer Name")]
        public string EngineerName { get; set; } = "ASRAF ALI";

        [Display(Name = "Case ID")]
        public string? CaseId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Activity Start Time")]
        public DateTime ActivityStartTime { get; set; }   // ✅ matches SQL DATETIME

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Activity End Time")]
        public DateTime ActivityEndTime { get; set; }     // ✅ matches SQL DATETIME

        [DataType(DataType.Time)]
        [Display(Name = "Break Duration")]
        public TimeSpan? BreakDuration { get; set; }      // ✅ matches SQL TIME

        [DataType(DataType.Time)]
        [Display(Name = "Time Track")]
        public TimeSpan? TimeTrack { get; set; }          // ✅ matches SQL TIME (computed)

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Sub-Category")]
        public string? SubCategory { get; set; }

        [Required]
        [Display(Name = "Case Type")]
        public string CaseType { get; set; } = string.Empty;

        [Display(Name = "Case Priority")]
        public string? CasePriority { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Activity Details")]
        public string? ActivityDetails { get; set; }

        [Required]
        public string Status { get; set; } = "Open";

        [Display(Name = "Handover Reason")]
        public string? HandoverReason { get; set; }

        [Display(Name = "Elevation Call Reason")]
        public string? ElevationCallReason { get; set; }

        [Display(Name = "ITSM Update")]
        public string? ITSMUpdate { get; set; }

        public string? Remarks { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
    }
}
