using MattsWorld.TellUs.Core;

namespace MattsWorld.TellUs.Events
{
    public abstract class TellUsEvent : ITellUsEvent
    {
        public string Type => GetType().AssemblyQualifiedName;
        public string Name => GetType().Name;
        public abstract string Category { get; }
        public abstract string Area { get; }
    }
}
