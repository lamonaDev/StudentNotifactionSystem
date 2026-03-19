using System;
using System.Collections.Generic;

namespace StudentNotifactionSystem.Main;

public class CourseChannel
{
    public string CourseName { get; }
    public event EventHandler<NotificationEventArgs> NotificationPublished;
    private readonly List<string> _subscriptionsHistory;
    public CourseChannel(string courseName)
    {
        CourseName = courseName;
        this._subscriptionsHistory = new List<string>();
    }

    public void Subscribe(Student student)
    {
        NotificationPublished += student.Receive!;
        this._subscriptionsHistory.Add(student.Name);
    }

    public void Unsubscribe(Student student)
    {
        NotificationPublished -= student.Receive!;
        this._subscriptionsHistory.Remove(student.Name);
    }

    public void Publish(string title, string message)
    {
        var eventArgs = new NotificationEventArgs(this.CourseName, title, message);
        
        Console.WriteLine($"\n{'='.ToString().PadRight(50, '=')}");
        Console.WriteLine($"PUBLISHING to {CourseName}");
        Console.WriteLine($"Content: {eventArgs}");
        Console.WriteLine($"Subscribers Count: {GetSubscribersCount()}");
        Console.WriteLine($"{'='.ToString().PadRight(50, '=')}\n");

        NotificationPublished?.Invoke(this, eventArgs);
    }

    public int GetSubscribersCount()
    {
        return NotificationPublished?.GetInvocationList().Length ?? 0;
    }
}
