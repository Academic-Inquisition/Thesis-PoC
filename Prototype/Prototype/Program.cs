using CommandLine;
using Prototype.node;
using Prototype.node.components;

namespace Prototype
{
    /// <summary>
    /// The main program class responsible for running the test with multiple nodes.
    /// </summary>
    public class Program
    {
        private static int Nodes = 5;

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        static void Main(string[] args)
        {
            Options? options = null;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => options = o);
            // Create/Start the threads
            if (options != null)
            {
                NodeClient client = new NodeClient(options);
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

                while (client.finishedNodeInfo.Count < options.Nodes) {}

                string filename = $"{options.QueueType.ToLower()}_output_{DateTime.Now.ToString("ddMMyyyy")}.csv";
                bool needsNewFile = false;
                if (Path.Exists(options.Output_Path) == false)
                {
                    Directory.CreateDirectory(options.Output_Path);
                }

                if (File.Exists(options.Output_Path + filename) == false)
                {
                    needsNewFile = true;
                }

                using (StreamWriter output = new StreamWriter(options.Output_Path + filename, true))
                {
                    if (needsNewFile) output.WriteLine("Finished Time(id), Node(id), Time to Execute(in ms), Rounds(amount)");
                    foreach (string time in client.finishedNodeInfo)
                    {
                        output.WriteLine(time);
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }

    /// <summary>
    /// Class representing the options that can be specified via command-line arguments.
    /// </summary>
    public class Options
    {
        [Option('p', "path", HelpText = "Default output path for the finalized file with times [Default: ./output/]", Required = false)]
        public string Output_Path { get; set; } = $"./output/";

        [Option('n', "nodes", HelpText = "The amount of Nodes to run [Default: 5]", Required = false)]
        public int Nodes { get; set; } = 5;

        [Option('r', "rounds", HelpText = "The amount of rounds to run [Default: 10000]", Required = false)]
        public int Rounds { get; set; } = 10_000;

        [Option('t', "type", HelpText = "The queue-type to run the test for [Default: 'Queue']\n\tOptions: [BlockingCollection, Channel, ConcurrentBag, ConcurrentQueue, Queue, RingBuffer]", Required = false)]
        public string QueueType { get; set; } = "Queue";

        [Option('d', "debug", HelpText = "Enable Debug Logging [Default: false]", Required = false)]
        public bool Debug { get; set; } = false;
    }
}

