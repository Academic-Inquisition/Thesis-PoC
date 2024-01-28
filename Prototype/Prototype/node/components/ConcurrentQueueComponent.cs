using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components
{
    public class ConcurrentQueueComponent : IQueueComponent
    {
        private Queue<string> queue;
        private object lockObject = new object();

        public int Count => queue.Count;

        public bool IsEmpty => queue.Count == 0;

        public ConcurrentQueueComponent(int capacity)
        {
            this.queue = new Queue<string>(capacity);
        }

        public void Enqueue(string message)
        {
            lock (lockObject)
            {
                queue.Enqueue(message);
            }
        }

        public string Dequeue()
        {
            lock (lockObject)
            {
                return queue.Dequeue();
            }
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new ConcurrentQueueComponent(capacity * 3);
        }
    }
}
