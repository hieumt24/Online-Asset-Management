namespace AssetManagement.Application.Models.DTOs.Reports.Responses
{
    public class ReportResponseDto
    {
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int Total { get; set; }
        public int Assigned { get; set; }
        public int Available { get; set; }
        public int NotAvailable { get; set; }
        public int WaitingForRecycling { get; set; }
        public int Recycled { get; set; }
    }
}