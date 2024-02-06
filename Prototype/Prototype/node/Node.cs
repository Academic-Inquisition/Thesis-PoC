using Prototype.node.components;
using System.Diagnostics;

namespace Prototype.node
{
    /// <summary>
    /// Represents a node in the prototype system.
    /// </summary>
    public class Node
    {
        public NodeClient client;
        public Options options;

        public int Id { get; set; }
        public IQueueComponent Queue { get; set; }
        public Thread thread { get; set; }

        public bool isLeader;

        public bool hasProposed = false;
        public long lastSent = DateTime.Now.Ticks;

        public int consensus = 0;
        public int votes = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="client">The node client.</param>
        /// <param name="id">The node identifier.</param>
        /// <param name="queue">The queue component.</param>
        public Node(NodeClient client, int id, IQueueComponent queue)
        {
            this.client = client;
            Id = id;
            Queue = queue;
            thread = new Thread(Run);
            thread.Name = $"Node {Id}";
            isLeader = id == 0 ? true : false;
            options = client.Options;
        }

        /// <summary>
        /// Main method representing the node's operation.
        /// </summary>
        public void Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (isLeader && !hasProposed)
                {
                    client.BroadcastMessage(NodeClient.CreateMessage(Id, "x"));
                    this.hasProposed = true;
                    lastSent = DateTime.Now.Ticks;
                }

                string? deq = null;
                if (Queue.Count > 0)
                {
                    deq = Queue.Dequeue();
                }

                if (deq != null)
                {
                    string[] s = deq.Split(':');
                    (int sender, string message) = (int.Parse(s[0]), s[1]);
                    if (options.Debug) Console.WriteLine($"Node {Id} received message from Node {sender}: {message}");
                    if (message == "x")
                    {
                        client.BroadcastMessage(NodeClient.CreateMessage(Id, "y"));
                    }
                    if (message == "y")
                    {
                        votes++;
                        if (votes >= 4)
                        {
                            consensus++;
                            votes = 0;
                            if (isLeader) hasProposed = false;
                        }
                    }
                    if (consensus == options.Rounds)
                    {
                        if (options.Debug) Console.WriteLine($"Node {Id} shutting down...");
                        sw.Stop();
                        client.finishedNodes++;
                        Console.WriteLine($"Node {Id} took {sw.ElapsedMilliseconds}ms to reach {options.Rounds} consensus rounds");
                        client.finishedNodeTimes.Add($"{client.Iteration}, {Id}, {sw.ElapsedMilliseconds}, {options.Rounds}");
                        break;
                    }
                }
            }

        }
    }
}
