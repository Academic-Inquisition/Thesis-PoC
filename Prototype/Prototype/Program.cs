using Prototype.node;
using Prototype.node.components;
using Prototype.node.components.finished;

namespace Prototype
{
    public class Program
    {
        private static int Nodes = 5;

        static void Main(string[] args)
        {
            // Create/Start the threads
            NodeClient client = new NodeClient(Nodes);
            Console.WriteLine($"Starting {Node.consensusRounds} Rounds of Negotiation");
            //client.InitSystem(QueueComponent.CreateQueueManager);
            //client.InitSystem(BlockingCollectionQueueComponent.CreateQueueManager);
            //client.InitSystem(ConcurrentQueueComponent.CreateQueueManager);
            //client.InitSystem(ChannelQueueComponent.CreateQueueManager);
            client.InitSystem(ConcurrentBagQueueComponent.CreateQueueManager);
        }
    }
}
