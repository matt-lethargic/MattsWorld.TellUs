namespace MattsWorld.TellUs.Core
{
    public interface ITellUsEvent
    {
        string Type { get; }
        string Name { get; }
        string Category { get; }
    }
}
