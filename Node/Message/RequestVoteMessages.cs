using Node.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Message
{
    public class RequestVoteMessages
    {

        public class RequestVoteMessage : BaseMessage
        {
            public RequestVoteMessage(SystemNode Sender, SystemNode Receiver, Dictionary<string, object> Data, int Term) : base(Sender, Receiver, Data, Term)
            {
                this.MessageType = BaseMessage.Type.RequestVote;
            }
        }

        public class RequestVoteResponseMessage : BaseMessage
        {
            public RequestVoteResponseMessage(SystemNode Sender, SystemNode Receiver, Dictionary<string, object> Data, int Term) : base(Sender, Receiver, Data, Term)
            {
                this.MessageType = BaseMessage.Type.RequestVoteResponse;
            }
        }
    }
}
