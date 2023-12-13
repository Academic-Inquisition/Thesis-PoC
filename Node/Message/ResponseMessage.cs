using Node.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Message
{
    public class ResponseMessage : BaseMessage
    {
        public ResponseMessage(SystemNode Sender, SystemNode Receiver, Dictionary<string, object> Data, int Term) : base(Sender, Receiver, Data, Term)
        {
            this.MessageType = BaseMessage.Type.Response;
        }
    }
}
