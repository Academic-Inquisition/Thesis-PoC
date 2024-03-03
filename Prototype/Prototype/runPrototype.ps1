$customValues = Read-Host "Do you want to enter custom values? [yes, no]"
$runs = Read-Host "How many rounds would you like to run the prototype for?"

if (-not ([int]::TryParse($runs, [ref]$null))) {
  Write-Host "Please enter a valid integer value for runs."
  exit
}

$runs = [int]$runs

if ($customValues.ToLower() -eq "yes") {
  $out_path = Read-Host "Default output path for the finalized file with times [Default: .output/]"
  if ($out_path -eq "") { $out_path = "./output/" }

  $queue_type = Read-Host "The queue-type to run the test for [Default: 'Queue']\n\tOptions: [BlockingCollection, Channel, ConcurrentBag, ConcurrentQueue, Queue, RingBuffer]"
  if ($queue_type -eq "" -or -not ("BlockingCollection", "Channel", "ConcurrentBag", "ConcurrentQueue", "Queue", "RingBuffer" -contains $queue_type)) {
    $queue_type = "Queue"
  }

  $debug = Read-Host "Enable Debug Logging [Default: false]"
  $debug = $debug.ToLower() -eq "true" -or $debug.ToLower() -ne ""

  $nodes = Read-Host "The amount of Nodes to run [Default: 5]"
  if ($nodes -eq "" -or -not ([int]::TryParse($nodes, [ref]$null))) {
    $nodes = 5
  } else {
    $nodes = [int]$nodes
  }

  $rounds = Read-Host "The amount of rounds to run [Default: 10000]"
  if ($rounds -eq "" -or -not ([int]::TryParse($rounds, [ref]$null))) {
    $rounds = 10000
  } else {
    $rounds = [int]$rounds
  }

  for ($i = 1; $i -le $runs; $i++) {
    & ".\bin\Debug\net8.0\Prototype.exe" -p $out_path -n $nodes -r $rounds -t $queue_type
  }
} else {
  for ($i = 1; $i -le $runs; $i++) {
    & ".\bin\Debug\net8.0\Prototype.exe"
  }
}

