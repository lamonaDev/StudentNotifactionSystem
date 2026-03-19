namespace StudentNotifactionSystem.Main;

public class NotificationEventArgs : EventArgs
{
    public string Title { get; }
    public string Message { get; }
    public DateTime Timestamp { get; }
    public string CourseName { get; }
    public NotificationEventArgs(string courseName, string title, string message)
    {
        CourseName = courseName;
        Title = title;
        Message = message;
        Timestamp = DateTime.Now;
    }
    public override string ToString()
    {
        return $"[{Timestamp:HH:mm:ss}] {CourseName} - {Title}: {Message}";
    }
}