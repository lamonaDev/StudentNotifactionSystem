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
This implementation demonstrates clean separation of concerns, proper use of C# events, and the Observer pattern's power in building loosely coupled, event-driven systems.
```
