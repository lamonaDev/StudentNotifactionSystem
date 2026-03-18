# Student Notification System
## Console Application using Observer Pattern & Events

**Project Type:** Console Application (C# .NET)
**Design Pattern:** Observer Pattern (Publish-Subscribe)
**Communication Mechanism:** C# Events & EventArgs

---

## 1. Project Overview

A simple notification system where a `CourseChannel` (Subject) broadcasts messages to multiple `StudentSubscriber` objects (Observers). When a new notification is published, all subscribed students receive the message simultaneously through event-driven communication.

### Key Features
- **Loose Coupling**: Students don't know about each other; Channel doesn't know specific student implementations
- **Dynamic Subscription**: Students can join (subscribe) or leave (unsubscribe) at runtime
- **Rich Notification Data**: Includes title, message content, and timestamp
- **Thread-Safe Events**: Using standard C# event patterns with null-conditional invocation

---

## 2. Architecture Design

### Class Structure

```
┌─────────────────────┐
│  NotificationEvent  │◄── Event Data Container
│  (EventArgs)        │    - Title : string
│                     │    - Message : string
│                     │    - Timestamp : DateTime
└─────────────────────┘
          ▲
          │ uses
┌─────────────────────┐     subscribes to      ┌─────────────────────┐
│   CourseChannel     │◄───────────────────────►│  StudentSubscriber  │
│     (Subject)       │                         │     (Observer)      │
│                     │                         │                     │
│ + NotificationEvent │                         │ - Name : string     │
│ + Publish()         │                         │ - Receive()         │
│ + Subscribe()       │                         │                     │
│ + Unsubscribe()     │                         │                     │
└─────────────────────┘                         └─────────────────────┘
```

---

## 3. Implementation

### File 1: NotificationEventArgs.cs
```csharp
using System;

namespace StudentNotificationSystem
{
    /// <summary>
    /// Event data container for course notifications
    /// </summary>
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
}
```

### File 2: CourseChannel.cs (Subject)
```csharp
using System;
using System.Collections.Generic;

namespace StudentNotificationSystem
{
    /// <summary>
    /// The Subject/Publisher that manages notifications
    /// </summary>
    public class CourseChannel
    {
        public string CourseName { get; }

        // The Event - using EventHandler<T> pattern
        public event EventHandler<NotificationEventArgs> NotificationPublished;

        private readonly List<string> _subscribersHistory;

        public CourseChannel(string courseName)
        {
            CourseName = courseName;
            _subscribersHistory = new List<string>();
        }

        /// <summary>
        /// Subscribe a student to notifications
        /// </summary>
        public void Subscribe(StudentSubscriber student)
        {
            NotificationPublished += student.ReceiveNotification;
            _subscribersHistory.Add(student.Name);
            Console.WriteLine($"[SYSTEM] {student.Name} subscribed to {CourseName}");
        }

        /// <summary>
        /// Unsubscribe a student from notifications
        /// </summary>
        public void Unsubscribe(StudentSubscriber student)
        {
            NotificationPublished -= student.ReceiveNotification;
            _subscribersHistory.Remove(student.Name);
            Console.WriteLine($"[SYSTEM] {student.Name} unsubscribed from {CourseName}");
        }

        /// <summary>
        /// Publish notification to all subscribers
        /// </summary>
        public void PublishNotification(string title, string message)
        {
            var eventArgs = new NotificationEventArgs(CourseName, title, message);

            Console.WriteLine($"\n{'='.ToString().PadRight(50, '=')}");
            Console.WriteLine($"PUBLISHING to {CourseName}");
            Console.WriteLine($"Content: {eventArgs}");
            Console.WriteLine($"Subscribers Count: {GetSubscribersCount()}");
            Console.WriteLine($"{'='.ToString().PadRight(50, '=')}\n");

            // Thread-safe event invocation
            NotificationPublished?.Invoke(this, eventArgs);
        }

        public int GetSubscribersCount()
        {
            return NotificationPublished?.GetInvocationList().Length ?? 0;
        }
    }
}
```

### File 3: StudentSubscriber.cs (Observer)
```csharp
using System;

namespace StudentNotificationSystem
{
    /// <summary>
    /// The Observer/Subscriber that receives notifications
    /// </summary>
    public class StudentSubscriber
    {
        public string Name { get; }
        public string StudentId { get; }
        public int NotificationsReceived { get; private set; }

        public StudentSubscriber(string name, string studentId)
        {
            Name = name;
            StudentId = studentId;
            NotificationsReceived = 0;
        }

        /// <summary>
        /// Event handler method - called when notification is published
        /// </summary>
        public void ReceiveNotification(object sender, NotificationEventArgs e)
        {
            NotificationsReceived++;

            Console.WriteLine($"[RECEIVED] Student: {Name} (ID: {StudentId})");
            Console.WriteLine($"           Notification #{NotificationsReceived}");
            Console.WriteLine($"           Details: {e.Title}");
            Console.WriteLine($"           Message: {e.Message}");
            Console.WriteLine($"           Time: {e.Timestamp:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine(new string('-', 50));
        }

        public void DisplayStats()
        {
            Console.WriteLine($"[STATS] {Name} has received {NotificationsReceived} notifications total.");
        }
    }
}
```

### File 4: Program.cs (Demo)
```csharp
using System;
using System.Threading;

namespace StudentNotificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Student Notification System");
            Console.WriteLine("===========================\n");

            // Create Subject (Course Channel)
            var mathCourse = new CourseChannel("Advanced Mathematics - MATH101");

            // Create Observers (Students)
            var student1 = new StudentSubscriber("Alice Johnson", "S001");
            var student2 = new StudentSubscriber("Bob Smith", "S002");
            var student3 = new StudentSubscriber("Charlie Brown", "S003");

            // Scenario 1: Single subscriber
            Console.WriteLine("\n>>> Scenario 1: Alice subscribes");
            mathCourse.Subscribe(student1);
            mathCourse.PublishNotification("Assignment Posted", "Please complete Chapter 5 exercises by Friday.");

            Thread.Sleep(1000); // Simulate time delay

            // Scenario 2: Multiple subscribers
            Console.WriteLine("\n>>> Scenario 2: Bob and Charlie subscribe");
            mathCourse.Subscribe(student2);
            mathCourse.Subscribe(student3);

            mathCourse.PublishNotification("Exam Schedule", "Mid-term exam will be held next Monday at 10:00 AM.");

            Thread.Sleep(1000);

            // Scenario 3: Unsubscribe
            Console.WriteLine("\n>>> Scenario 3: Bob unsubscribes");
            mathCourse.Unsubscribe(student2);

            mathCourse.PublishNotification("Grade Published", "Your assignment grades are now available in the portal.");

            // Final Statistics
            Console.WriteLine("\n\n>>> Final Statistics");
            Console.WriteLine("====================");
            student1.DisplayStats();
            student2.DisplayStats(); // Should show 2 (not 3)
            student3.DisplayStats();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
```

---

## 4. Expected Output

```
Student Notification System
===========================


>>> Scenario 1: Alice subscribes
[SYSTEM] Alice Johnson subscribed to Advanced Mathematics - MATH101

==================================================
PUBLISHING to Advanced Mathematics - MATH101
Content: [14:32:15] Advanced Mathematics - MATH101 - Assignment Posted: Please complete Chapter 5 exercises by Friday.
Subscribers Count: 1
==================================================

[RECEIVED] Student: Alice Johnson (ID: S001)
           Notification #1
           Details: Assignment Posted
           Message: Please complete Chapter 5 exercises by Friday.
           Time: 2024-10-15 14:32:15
--------------------------------------------------

>>> Scenario 2: Bob and Charlie subscribe
[SYSTEM] Bob Smith subscribed to Advanced Mathematics - MATH101
[SYSTEM] Charlie Brown subscribed to Advanced Mathematics - MATH101

==================================================
PUBLISHING to Advanced Mathematics - MATH101
Content: [14:32:16] Advanced Mathematics - MATH101 - Exam Schedule: Mid-term exam will be held next Monday at 10:00 AM.
Subscribers Count: 3
==================================================

[RECEIVED] Student: Alice Johnson (ID: S001)
           Notification #2
           Details: Exam Schedule
           Message: Mid-term exam will be held next Monday at 10:00 AM.
           Time: 2024-10-15 14:32:16
--------------------------------------------------
[RECEIVED] Student: Bob Smith (ID: S002)
           Notification #1
           Details: Exam Schedule
           Message: Mid-term exam will be held next Monday at 10:00 AM.
           Time: 2024-10-15 14:32:16
--------------------------------------------------
[RECEIVED] Student: Charlie Brown (ID: S003)
           Notification #1
           Details: Exam Schedule
           Message: Mid-term exam will be held next Monday at 10:00 AM.
           Time: 2024-10-15 14:32:16
--------------------------------------------------

>>> Scenario 3: Bob unsubscribes
[SYSTEM] Bob Smith unsubscribed from Advanced Mathematics - MATH101

==================================================
PUBLISHING to Advanced Mathematics - MATH101
Content: [14:32:17] Advanced Mathematics - MATH101 - Grade Published: Your assignment grades are now available in the portal.
Subscribers Count: 2
==================================================

[RECEIVED] Student: Alice Johnson (ID: S001)
           Notification #3
           Details: Grade Published
           Message: Your assignment grades are now available in the portal.
           Time: 2024-10-15 14:32:17
--------------------------------------------------
[RECEIVED] Student: Charlie Brown (ID: S003)
           Notification #2
           Details: Grade Published
           Message: Your assignment grades are now available in the portal.
           Time: 2024-10-15 14:32:17
--------------------------------------------------


>>> Final Statistics
====================
[STATS] Alice Johnson has received 3 notifications total.
[STATS] Bob Smith has received 2 notifications total.
[STATS] Charlie Brown has received 2 notifications total.

Press any key to exit...
```

---

## 5. Key Design Decisions

### EventHandler<T> Pattern
Using `EventHandler<NotificationEventArgs>` instead of custom delegates provides:
- Thread-safety conventions
- Standard .NET event pattern compliance
- Compatibility with Windows Forms/WPF if extended later

### Loose Coupling
- `CourseChannel` knows only that it has subscribers (via event), not concrete `StudentSubscriber` types
- Students know only they receive `NotificationEventArgs`, not the internal workings of the Course

### Memory Management
- Unsubscription (`-=`) prevents memory leaks by removing references
- Event args are lightweight data transfer objects (DTOs)

### Extensibility
- Easy to add new notification types by extending `NotificationEventArgs`
- Easy to add new observer types (Instructor, Administrator) implementing the same event signature

---

## 6. Project Structure

```
StudentNotificationSystem/
├── Program.cs                 # Entry point & demo scenarios
├── CourseChannel.cs           # Subject/Publisher
├── StudentSubscriber.cs       # Observer/Subscriber
└── NotificationEventArgs.cs   # Event data contract
```

**Build Command:**
```bash
dotnet new console -n StudentNotificationSystem
dotnet run
```

This implementation demonstrates clean separation of concerns, proper use of C# events, and the Observer pattern's power in building loosely coupled, event-driven systems.
```