using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components
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
