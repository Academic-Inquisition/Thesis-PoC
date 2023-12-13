using Node.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Node.Message.RequestVoteMessages;

namespace Node.Node.State
{
    public class Voter : BaseState
    {
        public SystemNode LastVote { get; set; }

        public override Tuple<BaseState, object> OnAppendEntries(BaseMessage Message)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BaseState, object> OnResponseReceived(BaseMessage Message)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BaseState, object> OnVoteReceived(BaseMessage Message)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BaseState, object> OnVoteRequest(BaseMessage Message)
        {
            if (this.LastVote == null && Message.Data.ContainsKey("lastLogIndex") ? ((int)Message.Data["lastLogIndex"]) >= this.Node.LastLogIndex : false)
            {
                this.LastVote = Message.Sender;
                this.SendResponseMessage(Message);
            }
            else
            {
                this.SendResponseMessage(Message, false);
            }

            return Tuple.Create(((BaseState)this), null as object);
        }

        public override void SendResponseMessage(BaseMessage Message, bool Response = true)
        {
            var RespMessage = new ResponseMessage(Node, Message.Sender, new Dictionary<string, object> {
                { "response", Response }
            }, Node.CurrentTerm);
            Node.SendMessageResponse(RespMessage);
        }
    }
}
