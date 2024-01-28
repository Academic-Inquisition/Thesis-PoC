using Prototype.node;

namespace Prototype
{
    public class Program
    {
        public static int Nodes = 5;
        public static int RunDuration = 5000;
        
        static void Main(string[] args)
        {
            // Create/Start the threads
            NodeClient client = new NodeClient(Nodes);
            client.InitSystem(QueueComponent.CreateQueueManager);

            //NodeClient client2 = new(Nodes);
            //client2.InitSystem(QueueComponent.CreateQueueManager);

            /**
            client = new(Nodes);
            client.InitSystem(QueueComponent.CreateQueueManager);

            client = new(Nodes);
            client.InitSystem(QueueComponent.CreateQueueManager);

            client = new(Nodes);
            client.InitSystem(QueueComponent.CreateQueueManager);
            */
        }
    }
}
