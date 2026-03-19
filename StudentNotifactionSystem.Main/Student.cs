using System;

namespace StudentNotifactionSystem.Main;

public class Student
{
    public string Name { get; private  set; }
    public string StudentId { get; private  set; }
    /// <summary>
    /// the number of notifications received to student
    /// </summary>
    public int NotficationsReceived { get; private  set; }

    public Student(string name, string studentId)
    {
        this.Name = name;
        this.StudentId = studentId;
        this.NotficationsReceived = 0;
    }

    public void Receive(object sender, NotificationEventArgs e)
    {
        NotficationsReceived++;
        Console.WriteLine($"[RECEIVED] Student: {Name} (ID: {StudentId})");
        Console.WriteLine($"           Notification #{this.NotficationsReceived}");
        Console.WriteLine($"           Title: {e.Title}");
        Console.WriteLine($"           Message: {e.Message}"); 
        Console.WriteLine($"           Time: {e.Timestamp:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine(new string('-', 50));
    }
    public void DisplayStats()
    {
        Console.WriteLine($"[STATS] {Name} has received {this.NotficationsReceived} notifications total.");
    }
}