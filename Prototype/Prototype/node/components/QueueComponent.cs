using Prototype.node.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node
{
    internal class QueueComponent : IQueueComponent
    {
        private Queue<string> queue;

        public int Count => queue.Count;

        public bool IsEmpty => queue.Count == 0;

        public QueueComponent(int capacity)
        {
            this.queue = new Queue<string>(capacity);
        }

        public void Enqueue(string message)
        {
            queue.Enqueue(message);
        }

        public string Dequeue()
        {
            return queue.Dequeue();
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new QueueComponent(capacity * 20);
        }
    }
}
