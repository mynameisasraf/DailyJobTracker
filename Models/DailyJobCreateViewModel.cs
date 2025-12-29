using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DailyJobTracker.Models
{
    public class DailyJobCreateViewModel
    {
        public DailyJob Job { get; set; } = new DailyJob();

        public string SelectedCategoryName { get; set; }
        public string SelectedSubCategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SubCategories { get; set; } = new List<SelectListItem>();
    }
}
