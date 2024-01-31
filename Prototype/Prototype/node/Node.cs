using Prototype.node.components;
using System.Diagnostics;

namespace Prototype.node
{
    public class Node
    {
        public NodeClient client;

        public int Id { get; set; }
        public IQueueComponent Queue { get; set; }
        public Thread Thread { get; set; }

        public int leader = 0;

        public long _lastSent = DateTime.Now.Ticks;
        private readonly object _lastSentLock = new();

        public static int consensusRounds = 10_000;
        public bool _isStartUp = true;
        private readonly object _isStartUpLock = new();
        public bool _onGoingRound = false;
        private readonly object _onGoingRoundLock = new();
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
                bool localIsStartUp;
                lock (_isStartUpLock)
                {
                    localIsStartUp = _isStartUp;
                }

                long localLastSent;
                lock (_lastSentLock)
                {
                    localLastSent = _lastSent;
                }

                if (localIsStartUp || Id == leader && DateTime.Now.Ticks > localLastSent)
                {
                    lock (_isStartUpLock)
                    {
                        if (_isStartUp) _isStartUp = false;
                    }

                    bool localOnGoingRound;
                    lock (_onGoingRoundLock)
                    {
                        localOnGoingRound = _onGoingRound;
                    }

                    if (!localOnGoingRound)
                    {
                        client.BroadcastMessage(NodeClient.CreateMessage(Id, "x"));
                    }

                    lock (_onGoingRoundLock)
                    {
                        _onGoingRound = true;
                    }

                    lock (_lastSentLock)
                    {
                        _lastSent = DateTime.Now.Ticks;
                    }
                }

                string? deq = null;
                lock (Queue)
                {
                    if (Queue.Count > 0)
                    {
                        deq = Queue.Dequeue();
                    }
                }

                if (deq != null)
                {
                    string[] s = deq.Split(':');
                    (int sender, string message) = (int.Parse(s[0]), s[1]);
                    //Console.WriteLine($"Node {Id} received message from Node {sender}: {message}");
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
                            lock (_onGoingRoundLock)
                            {
                                _onGoingRound = false;
                            }
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
