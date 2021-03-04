using System.Collections.Generic;

namespace ChatRoom.Server
{
    public class ChatRoom
    {
        public string Name { get; set; }

        public Dictionary<string, string> Users { get; set; }
    }

}
