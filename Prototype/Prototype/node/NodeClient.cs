using Prototype.node.components;

namespace Prototype.node
{
    public class NodeClient
    {
        public delegate IQueueComponent Function<In, IQueueComponent>(In input);

        public static readonly object _optionsLock = new();
        public Options Options;
        public static readonly object _iterationLock = new();
        public int Iteration = 0;
        private static readonly object _nodeQueuesLock = new();
        public HashSet<IQueueComponent> NodeQueues = [];

        public int finishedNodes = 0;
        public List<string> finishedNodeTimes = new();

        public NodeClient(int iteration, Options options)
        {
            Options = options;
            Iteration = iteration;
        }

        public void InitSystem(Function<int, IQueueComponent> factory)
        {
            Console.WriteLine($"Using Type: {Options.QueueType}");
            for (int i = 0; i < Options.Nodes; i++)
            {
                Node node = new Node(this, i, factory(Options.Nodes));
                lock (_nodeQueuesLock)
                {
                    NodeQueues.Add(node.Queue);
                }
                node.thread.Start();
            }
        }

        public void BroadcastMessage(string message)
        {
            List<IQueueComponent> queues;
            lock (_nodeQueuesLock)
            {
                queues = new List<IQueueComponent>(NodeQueues);
            }
            for (int i = 0; i < queues.Count; i++)
            {
                queues[i].Enqueue(message);
            }
        }

        public static string CreateMessage(int senderId, string message)
        {
            return $"{senderId}:{message}";
        }
    }
}
