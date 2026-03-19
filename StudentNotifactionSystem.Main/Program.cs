using System;
using System.Threading;

namespace StudentNotifactionSystem.Main;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Student Notification System");
        Console.WriteLine("===========================\n");

        var mathCourse = new CourseChannel("Advanced Mathematics - MATH101");

        var student1 = new Student("Alice Johnson", "S001");
        var student2 = new Student("Bob Smith", "S002");
        var student3 = new Student("Charlie Brown", "S003");

        Console.WriteLine("\n>>> Scenario 1: Alice subscribes");
        mathCourse.Subscribe(student1);
        mathCourse.Publish("Assignment Posted", "Please complete Chapter 5 exercises by Friday.");

        Thread.Sleep(1000);

        Console.WriteLine("\n>>> Scenario 2: Bob and Charlie subscribe");
        mathCourse.Subscribe(student2);
        mathCourse.Subscribe(student3);

        mathCourse.Publish("Exam Schedule", "Mid-term exam will be held next Monday at 10:00 AM.");

        Thread.Sleep(1000);

        Console.WriteLine("\n>>> Scenario 3: Bob unsubscribes");
        mathCourse.Unsubscribe(student2);

        mathCourse.Publish("Grade Published", "Your assignment grades are now available in the portal.");

        Console.WriteLine("\n\n>>> Final Statistics");
        Console.WriteLine("====================");
        student1.DisplayStats();
        student2.DisplayStats();
        student3.DisplayStats();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}