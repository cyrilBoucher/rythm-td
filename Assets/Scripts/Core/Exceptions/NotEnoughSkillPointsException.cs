using System;

[Serializable()]
public class NotEnoughSkillPointsException : Exception
{
    public NotEnoughSkillPointsException() : base() { }
    public NotEnoughSkillPointsException(string message) : base(message) { }
    public NotEnoughSkillPointsException(string message, Exception inner) : base(message, inner) { }

    protected NotEnoughSkillPointsException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
