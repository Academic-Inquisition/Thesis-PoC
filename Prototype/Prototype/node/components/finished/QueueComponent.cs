using Prototype.node.components;

namespace Prototype.node.components.finished
{
    internal class QueueComponent : IQueueComponent
    {
        private Queue<string> queue;
        private readonly object _queueLock = new();

        public int Count
        {
            get
            {
                lock (_queueLock)
                {
                    return queue.Count;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (_queueLock)
                {
                    return queue.Count == 0;
                }
            }
        }

        private QueueComponent(int capacity) => queue = new(capacity);

        public void Enqueue(string message)
        {
            lock (_queueLock)
            {
                queue.Enqueue(message);
            }
        }

        public string? Dequeue()
        {
            lock (_queueLock)
            {
                if (queue.Count > 0)
                {
                    return queue.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new QueueComponent(capacity * 2000);
        }
    }
}
