using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components
{
    public interface IQueueComponent
    {
        public int Count { get; }
        public bool IsEmpty { get; }

        public void Enqueue(string message);
        public string Dequeue();
        public static IQueueComponent CreateQueueManager(int capacity) { return null; }
    }
}
