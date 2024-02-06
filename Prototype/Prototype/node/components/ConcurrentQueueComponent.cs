using System.Collections.Concurrent;

namespace Prototype.node.components
{
    public class ConcurrentQueueComponent : IQueueComponent
    {
        private ConcurrentQueue<string> queue;

        public int Count => queue.Count;

        public bool IsEmpty => !queue.Any();

        private ConcurrentQueueComponent() => queue = new();

        public void Enqueue(string message)
        {
            queue.Enqueue(message);
        }

        public string? Dequeue()
        {
            queue.TryDequeue(out string result);
            return result;
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new ConcurrentQueueComponent();
        }
    }
}
