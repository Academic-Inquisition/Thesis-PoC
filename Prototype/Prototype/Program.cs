using Prototype.node;
using Prototype.node.components;
using CommandLine;

namespace Prototype
{
    public class Program
    {
        private static int Nodes = 5;

        static void Main(string[] args)
        {
            List<string> finishedNodeTimes = new();
            Options? options = null;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => options = o);
            // Create/Start the threads
            if (options != null)
            {
                for (int i = 0; i < options.Iterations; i++)
                {
                    NodeClient client = new NodeClient(i, options);
                    switch (options.QueueType)
                    {
                        case "BlockingCollection":
                            client.InitSystem(BlockingCollectionQueueComponent.CreateQueueManager);
                            break;
                        case "Channel":
                            client.InitSystem(ChannelQueueComponent.CreateQueueManager);
                            break;
                        case "ConcurrentBag":
                            client.InitSystem(ConcurrentBagQueueComponent.CreateQueueManager);
                            break;
                        case "ConcurrentQueue":
                            client.InitSystem(ConcurrentQueueComponent.CreateQueueManager);
                            break;
                        case "Queue":
                            client.InitSystem(QueueComponent.CreateQueueManager);
                            break;
                        case "RingBuffer":
                            client.InitSystem(RingBufferComponent.CreateQueueManager);
                            break;
                    }
                    while (client.finishedNodeTimes.Count < options.Nodes) { }
                    finishedNodeTimes.AddRange(client.finishedNodeTimes);
                }
                string filename = $"output_{DateTime.Now.ToString("ddMMyyyy_HH-mm-ss")}.csv";
                if (Path.Exists(options.Output_Path) == false)
                {
                    Directory.CreateDirectory(options.Output_Path);
                }
                using (StreamWriter output = new StreamWriter(options.Output_Path + filename))
                {
                    output.WriteLine("Iteration(id), Node(id), Time(in ms), Rounds(amount)");
                    foreach (string time in finishedNodeTimes)
                    {
                        output.WriteLine(time);
                    }
                }
            }
        }
    }

    public class Options
    {
        [Option('p', "path", HelpText = "Default output path for the finalized file with times [Default: ./]", Required = false)]
        public string Output_Path { get; set; } = $"../../../output/";

        [Option('n', "nodes", HelpText = "The amount of Nodes to run [Default: 5]", Required = false)]
        public int Nodes { get; set; } = 5;

        [Option('r', "rounds", HelpText = "The amount of rounds to run [Default: 10000]", Required = false)]
        public int Rounds { get; set; } = 10_000;

        [Option('i', "iterations", HelpText = "The amount of iterations of the test to run [Default: 1]", Required = false)]
        public int Iterations { get; set; } = 1;

        [Option('t', "type", HelpText = "The queue-type to run the test for [Default: 'Queue']\nOptions: [BlockingCollection, Channel, ConcurrentBag, ConcurrentQueue, Queue, RingBuffer]", Required = false)]
        public string QueueType { get; set; } = "Queue";

        [Option('d', "debug", HelpText = "Enable Debug Logging [Default: false]", Required = false)]
        public bool Debug { get; set; } = false;
    }
}
