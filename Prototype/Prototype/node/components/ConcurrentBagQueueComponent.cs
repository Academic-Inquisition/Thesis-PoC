using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components
{
    public class ConcurrentBagQueueComponent : IQueueComponent
    {

        private readonly ConcurrentBag<string> queue = new ConcurrentBag<string>();

        public int Count => queue.Count;

        public bool IsEmpty => queue.Count == 0;

        private ConcurrentBagQueueComponent(int capacity) { }

        public void Enqueue(string message)
        {
            queue.Add(message);
        }

        public string Dequeue()
        {
            while (true)
            {
                if (queue.TryTake(out var result)) return result;
                Thread.Yield();
            }
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new ConcurrentBagQueueComponent(capacity);
        }
    }
}
