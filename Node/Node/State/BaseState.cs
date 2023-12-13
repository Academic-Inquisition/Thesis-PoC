using Node.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Node.State
{
    public abstract class BaseState
    {
        public SystemNode Node { get; set; }
        public long CurrentTime { get; set; }
        public int Timeout { get; set; }
        public Random Rand = new Random();

        public Tuple<BaseState, object> OnMessage(BaseMessage Message)
        {
            BaseMessage.Type MessageType = Message.MessageType;
            if (Message.Term > Node.CurrentTerm)
            {
                Node.CurrentTerm = Message.Term;
            }
            else if (Message.Term < Node.CurrentTerm)
            {
                SendResponseMessage(Message, false);
                return new Tuple<BaseState, object>(this, null);
            }
            
            if (MessageType == BaseMessage.Type.AppendEntries)
            {
                return OnAppendEntries(Message);
            }
            else if (MessageType == BaseMessage.Type.RequestVote)
            {
                return OnVoteRequest(Message);
            }
            else if (MessageType == BaseMessage.Type.RequestVoteResponse)
            {
                return OnVoteReceived(Message);
            }
            else if (MessageType == BaseMessage.Type.Response)
            {
                return OnResponseReceived(Message);
            }
            return null;
        }

        public abstract Tuple<BaseState, object> OnResponseReceived(BaseMessage Message);
        public abstract Tuple<BaseState, object> OnVoteReceived(BaseMessage Message);
        public abstract Tuple<BaseState, object> OnVoteRequest(BaseMessage Message);
        public abstract Tuple<BaseState, object> OnAppendEntries(BaseMessage Message);

        public virtual void SendResponseMessage(BaseMessage Message, bool Response = true)
        {
            var RespMessage = new ResponseMessage(Node, Message.Sender, new Dictionary<string, object> {
                { "response", Response },
                { "currentTerm", Node.CurrentTerm }
            }, Node.CurrentTerm);
            Node.SendMessageResponse(RespMessage);
        }

        private long NextTimeout()
        {
            this.CurrentTime = DateTime.Now.Ticks;
            return this.CurrentTime + Rand.NextInt64(Timeout, 2 * Timeout);
        }
    }
}
