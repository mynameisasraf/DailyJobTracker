namespace DailyJobTracker.Models
{
    public class Category
    {
        public int Id { get; set; }

        // Main category name (e.g., "Azure Compute")
        public string CategoryName { get; set; } = string.Empty;

        // Subcategory name (e.g., "Virtual Machine Management")
        public string SubCategoryName { get; set; } = string.Empty;
    }
}
