namespace Prototype
{
    public class Program
    {
        public static int RunDuration = 5000;
        public static int N = 5;
        public static HashSet<Queue<string>> NodeQueues = [];
        
        static void Main(string[] args)
        {
            // Create/Start the threads
            InitSystem();
            // Simulate the broadcast of a message
            BroadcastMessage(0, CreateMessage(0, "Do we like trains?"));
            Thread.Sleep(RunDuration);
            ShutdownSystem();
        }

        private static void InitSystem()
        {
            for (int i = 0; i < N; i++)
            {
                Node node = new Node(i);
                NodeQueues.Add(node.Queue);
                node.Thread.Start();
            }
        }

        public static void BroadcastMessage(int senderId, string message)
        {
            List<Queue<string>> queues = new(NodeQueues);
            for (int i = 0; i < N; i++)
            {
                if (i != senderId)
                {
                    queues[i].Enqueue(message);
                }
            }
        }

        private static void ShutdownSystem()
        {
            foreach (Queue<string> queue in NodeQueues)
            {
                queue.Enqueue(CreateMessage(0, "shutdown"));
            }
        }

        public static string CreateMessage(int senderId, string message)
        {
            return $"{senderId}:{message}";
        }
    }

    public class Node
    {
        public int Id { get; set; }
        public Queue<string> Queue { get; set; }
        public Thread Thread { get; set; }

        public int leader = 0;

        public long lastSent = DateTime.Now.Ticks + 25000000L;

        public Node(int id)
        {
            Id = id;
            Queue = new Queue<string>();
            Thread = new Thread(() => Run());
        }

        public void Run()
        {
            while (true)
            {
                if (Id == leader && DateTime.Now.Ticks > lastSent)
                {
                    Program.BroadcastMessage(Id, Program.CreateMessage(Id, "Are you alive?"));
                    lastSent = DateTime.Now.Ticks + 25000;
                }
                if (Queue.Count > 0)
                {
                    string[] s = Queue.Dequeue().Split(':');
                    (int sender, string message) = (int.Parse(s[0]), s[1]);
                    Console.WriteLine($"Node {Id} received message from Node {sender}: {message}");
                    if (message == "shutdown")
                    {
                        Console.WriteLine($"Node {Id} shutting down...");
                        break;
                    }
                    if (message == "Are you alive?")
                    {
                        Program.BroadcastMessage(Id, Program.CreateMessage(Id, "Yes"));
                    }
                }
                Thread.Sleep(Random.Shared.Next(100, 1000));
            }
        }
    }
}
