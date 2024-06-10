using System;
using System.Collections;

public interface IEventBinding<T> 
{
    public Action<T> Event { get; set; }
    public Action EventNoArgs { get; set; }
}
