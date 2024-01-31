using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components.finished
{
    public class ConcurrentBagQueueComponent : IQueueComponent
    {

        private readonly ConcurrentBag<string> queue = new ConcurrentBag<string>();

        public int Count => queue.Count;

        public bool IsEmpty => queue.Count == 0;

        private ConcurrentBagQueueComponent(int capacity)
        {
            // LockFreeQueue doesn't require capacity initialization
        }

        public void Enqueue(string message)
        {
            queue.Add(message);
        }

        public string Dequeue()
        {
            while (true)
            {
                if (queue.TryTake(out var result))
                    return result;
                // Add some backoff or wait strategy here if needed
                Thread.Yield(); // Or use other strategies like SpinWait, Sleep, etc.
            }
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new ConcurrentBagQueueComponent(capacity * 2000);
        }
    }
}
