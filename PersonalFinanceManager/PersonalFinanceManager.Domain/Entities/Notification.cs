namespace PersonalFinanceManager.Domain.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public required string Message { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
}
