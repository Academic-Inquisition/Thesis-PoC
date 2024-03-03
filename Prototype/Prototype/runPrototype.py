import subprocess

custom_values = input("Do you want to enter custom values? [yes, no]: ")
runs = input("How many rounds would you like to run the prototype for?: ")

try:
    runs = int(runs)
except ValueError:
    print("Please enter a valid integer value for runs.")
    exit()

if custom_values.lower() == "yes":
    out_path = input("Default output path for the finalized file with times [Default: ./output/]: ")
    out_path = out_path if out_path != "" else "./output/"

    queue_type = input("The queue-type to run the test for [Default: 'Queue']\n\tOptions: [BlockingCollection, Channel, ConcurrentBag, ConcurrentQueue, Queue, RingBuffer]: ")
    queue_type = queue_type if queue_type != "" and queue_type in ["BlockingCollection", "Channel", "ConcurrentBag", "ConcurrentQueue", "Queue", "RingBuffer"] else "Queue"

    debug = input("Enable Debug Logging [Default: false]: ")
    debug = debug.lower() == "true" or debug.lower() != ""

    nodes = input("The amount of Nodes to run [Default: 5]: ")
    nodes = int(nodes) if nodes != "" and nodes.isdigit() else 5

    rounds = input("The amount of rounds to run [Default: 10000]: ")
    rounds = int(rounds) if rounds != "" and rounds.isdigit() else 10000

    for i in range(1, runs + 1):
        subprocess.run([".\\bin\\Debug\\net8.0\\Prototype.exe", "-p", out_path, "-n", str(nodes), "-r", str(rounds), "-t", queue_type])
else:
    for i in range(1, runs + 1):
        subprocess.run([".\\bin\\Debug\\net8.0\\Prototype.exe"])
