using Node.Message;
using Node.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Node.Board
{

    public abstract class NodeBoard
    {
        public SystemNode Owner { get; set; }

        public abstract void PostMessage(BaseMessage Message);

        public abstract BaseMessage? GetMessage(BaseMessage Message);
    }

    public class MemoryBoard : NodeBoard
    {
        public List<BaseMessage> board;

        public override BaseMessage? GetMessage(BaseMessage Message)
        {
            if (board.Count > 0)
            {
                BaseMessage message = board[board.Count - 1];
                board.RemoveAt(board.Count - 1);
                return message;
            }
            return null;
        }

        public override void PostMessage(BaseMessage Message)
        {
            board.Add(Message);
            board = board.OrderByDescending(a => a.Timestamp).ToList();
        }
    }
}
