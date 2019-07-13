using System;

[Serializable()]
public class UpgradeAlreadyAcquiredException : Exception
{
    public UpgradeAlreadyAcquiredException() : base() { }
    public UpgradeAlreadyAcquiredException(string message) : base(message) { }
    public UpgradeAlreadyAcquiredException(string message, Exception inner) : base(message, inner) { }

    protected UpgradeAlreadyAcquiredException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
