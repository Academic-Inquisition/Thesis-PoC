namespace Prototype.node.components
{
    public interface IQueueComponent
    {
        public int Count { get; }
        public bool IsEmpty { get; }

        public void Enqueue(string message);
        public string? Dequeue();
        public static IQueueComponent CreateQueueManager(int capacity) { return null; }
    }
}
