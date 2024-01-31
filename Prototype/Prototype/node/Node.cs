using Prototype.node.components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.node
{
    public class Node
    {
        public NodeClient client;

        public int Id { get; set; }
        public IQueueComponent Queue { get; set; }
        public Thread Thread { get; set; }

        public int leader = 0;

        public long lastSent = DateTime.Now.Ticks + 25000000L;

        public static int consensusRounds = 1000;
        public bool isStartUp = true;
        public int consensus = 0;
        public int votes = 0;

        public Node(NodeClient client, int id, IQueueComponent queue)
        {
            this.client = client;
            Id = id;
            Queue = queue;
            Thread = new Thread(() => Run());
        }

        public void Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (isStartUp || Id == leader && DateTime.Now.Ticks > lastSent)
                {
                    if (isStartUp) isStartUp = false;
                    client.BroadcastMessage(client.CreateMessage(Id, "x"));
                    lastSent = DateTime.Now.Ticks + 25000;
                }
                if (Queue.Count > 0)
                {
                    string deq = Queue.Dequeue();
                    if (deq == null)
                    {
                        continue;
                    }
                    string[] s = deq.Split(':');
                    (int sender, string message) = (int.Parse(s[0]), s[1]);
                    //Console.WriteLine($"Node {Id} received message from Node {sender}: {message}");
                    if (message == "x")
                    {
                        client.BroadcastMessage(client.CreateMessage(Id, "y"));
                    }
                    if (message == "y")
                    {
                        votes++;
                        if (votes >= 4)
                        {
                            consensus++;
                            votes = 0;
                        }
                    }
                    if (consensus == consensusRounds)
                    {
                        //Console.WriteLine($"Node {Id} shutting down...");
                        //Console.WriteLine($"Node {Id}, Consensus: {consensus}");
                        break;
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"Node {Id} took {sw.ElapsedMilliseconds}ms to reach {consensusRounds} consensus rounds");
        }
    }
}
