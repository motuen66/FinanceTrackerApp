namespace FinanceTracker.Services.DTOs.TransactionDtos
{
    public class TransactionViewDto
    {
        public string Id { get; set; } = null!;
        public string Note { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
