using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Message
{
    public class AppendEntriesMessage : BaseMessage
    {
        public AppendEntriesMessage(string Sender, string Receiver, Dictionary<string, object> Data, int Term) : base(Sender, Receiver, Data, Term) 
        {
            this.MessageType = BaseMessage.Type.AppendEntries;
        }
    }
}
