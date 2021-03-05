using System;
using System.Linq;
using System.Threading.Tasks;
using WebScoket;

namespace ChatRoom.Server
{
    public class ChatRoomPipeline : IPipeline
    {
        private WebSocketContext _context;
        private string Username => ChatInstance.Users[_context.ClientRegistration.Id];

        public Task ExecuteAsync(WebSocketContext context, Action next)
        {
            _context = context;
            //context.Response.Write("Informe seu nickname:");

            if (string.IsNullOrWhiteSpace(context.Request.Body))
            {
                next();
            }
            if(context.Request.Body == Username)
            {
                context.Response.WriteLine("** Welcome to chat room, type -help to see all options.");
            }
            else if (context.Request.Body.ToLower().Contains("-help"))
            {
                context.Response.WriteLine("** Valid commands");
                context.Response.WriteLine("-u to Send a message to an user, ex: Hello -u John");
                context.Response.WriteLine("-p to send a message private, this command must be specified with -u");
                context.Response.WriteLine("-l to list all users");
                context.Response.WriteLine("-exit to quit");
            }
            else if(context.Request.Body.ToLower().Contains("-l"))
            {
                context.Response.WriteLine("** Users connected:");
                foreach (var item in ChatInstance.Users.Values)
                {
                    context.Response.WriteLine(item);
                }
            }
            else
            {
                var user = GetUser(context.Request.Body);
                var message = GetMessage(context.Request.Body);
                bool isPrivate = context.Request.Body.ToLower().Contains("-p");
                //bool isDirected = context.Request.Body.Contains("-u");
                //string client;

                //if(isDirected)

                if (string.IsNullOrWhiteSpace(user) == false)
                {
                    var id = ChatInstance.Users.Where(i => i.Value.ToLower() == user).Select(d => d.Key).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(id)) {
                        context.Response.WriteLine("** The specified nickname was not founded.");
                    }
                    else if(isPrivate)
                    {
                        context.Notify($"{Username} said privately: {message}", id);
                    }
                    else
                    {
                        context.Notify($"{Username} said to {user}: {message}");

                    }
                }
                else
                {
                    context.Notify($"{Username} said {message}");
                }

                next();

            }



            return Task.CompletedTask;
        }

        private string GetUser(string body)
        {
            body = body.Replace("-U", "-u");

            var split = body.Split("-u");

            if (split.Length <= 1)
            {
                return string.Empty;
            }
            else
            {
                return split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            }
        }

        private string GetMessage(string body)
        {
            body = body.Replace("-p", "").Replace("-P","");
            body = body.Replace("-U", "-u");

            if (body.Contains("-u") == false)
                return body;


            var split = body.Split("-u");
            
            if(split.Length > 1)
                return string.Join(" ", split[0], string.Join(" ", split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList()));
            else
                return string.Join(" ", split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList());
        }
    }


}
