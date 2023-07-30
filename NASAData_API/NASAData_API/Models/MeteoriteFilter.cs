using NASAData_API.Helpers;

namespace NASAData_API.Models
{
    public class MeteoriteFilter
    {
        public string? FromYears { get; set; } 
        public string? ToYears { get; set; } 
        public string? SearchName { get; set; } 
        public string? ClassSelect { get; set; }
        public string? SortBy { get; set; } 
        public string? SortDesc { get; set; }
        public MeteoriteFilter()
        {

        }

        public void Validate()
        {
            try
            {
                FromYears = !String.IsNullOrWhiteSpace(FromYears) ? DataTimeExtension.ParseYearToDateTime(FromYears.Trim()) : DateTime.MinValue.ToString("yyyy-MM-dd 00:00:00");
                ToYears =  !String.IsNullOrWhiteSpace(ToYears) ? DataTimeExtension.ParseYearToDateTime(ToYears.Trim()) : DateTime.MaxValue.ToString("yyyy-MM-dd 00:00:00");
                SearchName = !String.IsNullOrWhiteSpace(SearchName) ? SearchName.Trim() : "";
                ClassSelect = !String.IsNullOrWhiteSpace(ClassSelect) ?  ClassSelect.Trim() : "";
                SortBy = !String.IsNullOrWhiteSpace(SortBy) &&  typeof(MeteoriteView).GetProperties().Select(p => p.Name.ToLower()).Contains(SortBy.Trim().ToLower()) ? SortBy.Trim() : "";
                SortDesc = !String.IsNullOrWhiteSpace(SortDesc) && SortDesc.Trim()[0] == 't' ? "DESC" : "ASC";

            } catch(Exception ex)
            {
                throw new Exception("Invalid request parameters");
            }
        }

    }
}
