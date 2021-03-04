using System.Collections.Generic;

namespace ChatRoom.Server
{
    public static class ChatInstance
    {
        private static List<ChatRoom> _rooms = new List<ChatRoom>();
        public static List<ChatRoom> Rooms = new List<ChatRoom>();

        public const string EVERYONE = "Everyone";

        public static Dictionary<string, string> Users = new Dictionary<string, string>();
    }

}
