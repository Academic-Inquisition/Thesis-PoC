using Prototype.node.components;

namespace Prototype.node
{
    /// <summary>
    /// Represents a client managing nodes in the prototype system.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeClient"/> class.
        /// </summary>
        /// <param name="iteration">The iteration number.</param>
        /// <param name="options">The options for the client.</param>
        public NodeClient(int iteration, Options options)
        {
            Options = options;
            Iteration = iteration;
        }

        /// <summary>
        /// Initializes the system by creating nodes with the specified queue factory.
        /// </summary>
        /// <param name="factory">The factory function to create queues.</param>

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

        /// <summary>
        /// Broadcasts a message to all nodes in the system.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
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

        /// <summary>
        /// Creates a message with sender information.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="message">The message content.</param>
        /// <returns>The formatted message.</returns>
        public static string CreateMessage(int senderId, string message)
        {
            return $"{senderId}:{message}";
        }
    }
}
