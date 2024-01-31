using Disruptor;
using Disruptor.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node.components.unfinished
{
    /*
    public class DisruptorQueueComponent : IQueueComponent
    {
        private readonly RingBuffer<string> ringBuffer;
        private readonly ISequenceBarrierOptions sequenceBarrier;
        private readonly Disruptor<string> disruptor;
        private readonly ManualResetEventSlim consumerEvent = new(false);

        public int Count => (int)(ringBuffer.Cursor - ringBuffer.MinimumGatingSequence);

        public bool IsEmpty => Count == 0;

        private DisruptorQueueComponent(int capacity)
        {
            disruptor = new Disruptor<string>(() => "", capacity, TaskScheduler.Default);
            ringBuffer = disruptor.RingBuffer;
            sequenceBarrier = ringBuffer.NewBarrier();
            disruptor.HandleEventsWith(new MessageEventHandler(this));
            disruptor.Start();
        }

        public void Enqueue(string message)
        {
            queue.PublishEvent((ref string eventData, long sequence) => eventData = message);
        }

        public string Dequeue()
        {
            return queue.Take();
        }

        public static IQueueComponent CreateQueueManager(int capacity)
        {
            return new DisruptorQueueComponent(capacity * 2000);
        }
    }
    */
}
