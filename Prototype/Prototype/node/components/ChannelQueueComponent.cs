using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Prototype.node.components
{
    public class ChannelQueueComponent : IQueueComponent
    {
        private Channel<string> queue;

        public int Count => queue.Reader.Count;

        public bool IsEmpty => queue.Reader.Count == 0;

        private ChannelQueueComponent(int capacity) => queue = Channel.CreateBounded<string>(capacity);

        public void Enqueue(string message)
        {
            queue.Writer.TryWrite(message);
        }

        public string Dequeue()
        {
            queue.Reader.TryRead(out string result);
            return result;
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new ChannelQueueComponent(capacity * 2000);
        }
    }
}
