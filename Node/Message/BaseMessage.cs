using Node.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Message
{

    public class BaseMessage
    {
        public Type MessageType;
        public long Timestamp;
        public SystemNode Sender;
        public SystemNode Receiver;
        public Dictionary<string, object> Data;
        public int Term;

        public BaseMessage(SystemNode Sender, SystemNode Receiver, Dictionary<string, object> Data, int Term)
        {
            this.Timestamp = DateTime.Now.Ticks;
            this.Sender = Sender;
            this.Receiver = Receiver;
            this.Data = Data;
            this.Term = Term;
        }


        public enum Type
        {
            AppendEntries = 0,
            RequestVote = 1,
            RequestVoteResponse = 2,
            Response = 3
        }
    }
}
