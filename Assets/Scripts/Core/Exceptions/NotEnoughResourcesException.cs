using System;

[Serializable()]
public class NotEnoughResourcesException : Exception
{
    public NotEnoughResourcesException() : base() { }
    public NotEnoughResourcesException(string message) : base(message) { }
    public NotEnoughResourcesException(string message, Exception inner) : base(message, inner) { }

    protected NotEnoughResourcesException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
