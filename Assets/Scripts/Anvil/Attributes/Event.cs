using System;

[AttributeUsage(AttributeTargets.Method)]
public class Event : Attribute
{
    public string EventName;

    public Event(string eventName)
    {
        this.EventName = eventName;
    }
}