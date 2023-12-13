using Node.Message;
using Node.Node.Board;
using Node.Node.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Node
{
    public class SystemNode
    {
        public string Alias { get; set; }
        private BaseState State { get; set; } // Todo: Replace with State-type
        private List<string> Log { get; set; }
        private NodeBoard MessageBoard { get; set; }
        private List<SystemNode> Neighbors { get; set; }
        private int Total_Nodes {
            get { return Neighbors != null ? Neighbors.Count : 0; }
        }

        private int CommitIndex { get; set; }
        public int CurrentTerm { get; set; }

        private int LastApplied { get; set; }
        public int LastLogIndex { get; set; }
        private DateTime? LastLogTerm { get; set; }

        public SystemNode(string Alias, BaseState State, List<string> Log, NodeBoard MessageBoard, List<SystemNode> Neighbors)
        {
            this.Alias = Alias;
            this.State = State;
            this.Log = Log;
            this.MessageBoard = MessageBoard;
            this.Neighbors = Neighbors;
            this.LastLogTerm = null;
        }

        public void SendMessage(BaseMessage Message)
        {
            foreach(SystemNode Neighbor in Neighbors)
            {
                Message.Receiver = Neighbor.Alias;
                Neighbor.PostMessage(Message);
            }
        }

        public void SendMessageResponse(BaseMessage Message)
        {
            List<SystemNode> n = Neighbors.Where(
                Neighbor => Neighbor.Alias == Message.Receiver
            ).ToList();
            if (n.Count > 0) n[0].PostMessage(Message);
        }

        public void PostMessage(BaseMessage Message)
        {
            MessageBoard.PostMessage(Message);
        }

        public void OnMessage(BaseMessage Message)
        {

        }
    }
}
