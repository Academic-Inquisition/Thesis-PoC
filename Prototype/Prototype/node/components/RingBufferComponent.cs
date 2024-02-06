using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components
{
    public class RingBufferComponent : IQueueComponent
    {
        private readonly string[] buffer;
        private int head;
        private int tail;
        private readonly int capacity;
        private readonly object _bufferLock = new();

        public int Count
        {
            get
            {
                lock (_bufferLock)
                {
                    return (tail - head + capacity) % capacity;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (_bufferLock)
                {
                    return head == tail;
                }
            }
        }

        private RingBufferComponent(int capacity)
        {
            this.capacity = capacity;
            buffer = new string[capacity];
            head = 0;
            tail = 0;
        }

        public void Enqueue(string message)
        {
            lock (_bufferLock)
            {
                if ((tail + 1) % capacity == head)
                {
                    // Buffer is full
                    throw new InvalidOperationException("Buffer is full");
                }
                buffer[tail] = message;
                tail = (tail + 1) % capacity;
            }
        }

        public string? Dequeue()
        {
            lock (_bufferLock)
            {
                if (head == tail)
                {
                    // Buffer is empty
                    return null;
                }
                string message = buffer[head];
                head = (head + 1) % capacity;
                return message;
            }
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new RingBufferComponent(capacity * 2000);
        }
    }

}
