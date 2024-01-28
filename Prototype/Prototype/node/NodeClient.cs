using Prototype.node.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node
{
    public class NodeClient
    {
        public int nodes;
        public static HashSet<IQueueComponent> NodeQueues = [];

        public delegate IQueueComponent Function<In, IQueueComponent>(In input);

        public NodeClient(int nodes)
        {
            this.nodes = nodes;
        }

        public void InitSystem(Function<int, IQueueComponent> factory)
        {
            for (int i = 0; i < nodes; i++)
            {
                Node node = new Node(this, i, factory(this.nodes));
                NodeQueues.Add(node.Queue);
                node.Thread.Start();
            }
        }

        public void BroadcastMessage(int senderId, string message)
        {
            List<IQueueComponent> queues = new(NodeQueues);
            for (int i = 0; i < queues.Count; i++)
            {
                queues[i].Enqueue(message);
            }
        }

        public string CreateMessage(int senderId, string message)
        {
            return $"{senderId}:{message}";
        }
    }
}
