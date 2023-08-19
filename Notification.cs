namespace FinalProject
{
    public class Notification
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public enum NotificationType { Urgent, Reminder, Warning }
        public NotificationType notificationType { get; set; }
        public enum Module { Inventory, Sales, Supplier, User, Expense, Marketing }
        public Module notificationModule { get; set; }
    }
}
