using Prototype.node.components;

namespace Prototype.node
{
    public class NodeClient
    {
        private static readonly object _nodeQueuesLock = new();
        public delegate IQueueComponent Function<In, IQueueComponent>(In input);
        public int Nodes { get; }
        public static HashSet<IQueueComponent> NodeQueues = [];

        public NodeClient(int nodes) => Nodes = nodes;

        public void InitSystem(Function<int, IQueueComponent> factory)
        {
            for (int i = 0; i < Nodes; i++)
            {
                Node node = new Node(this, i, factory(Nodes));
                lock (_nodeQueuesLock)
                {
                    NodeQueues.Add(node.Queue);
                }
                node.Thread.Start();
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
