using System.Collections.Concurrent;

namespace Prototype.node.components.finished
{
    public class BlockingCollectionQueueComponent : IQueueComponent
    {
        private BlockingCollection<string> queue;

        public int Count => queue.Count;

        public bool IsEmpty => queue.Count == 0;

        private BlockingCollectionQueueComponent(int capacity) => queue = new(capacity);

        public void Enqueue(string message)
        {
            queue.Add(message);
        }

        public string Dequeue()
        {
            return queue.Take();
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new BlockingCollectionQueueComponent(capacity * 2000);
        }
    }
}
