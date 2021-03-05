using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScoket;

namespace ChatRoom.Server
{

    public class IdentificationPipeline : IPipeline
    {
        private WebSocketContext _context;
        private string Username => ChatInstance.Users[_context.ClientRegistration.Id];

        public Task ExecuteAsync(WebSocketContext context, Action next)
        {
            _context = context;

            if(context.Request.Body.ToLower().Contains("-exit"))
            {
                context.Notify($"{Username} has signed out");

                context.ClientRegistration.Quit();
                context.Response.Write("** You have signout");

                ChatInstance.Users.Remove(context.ClientRegistration.Id);

            }
            else if (ChatInstance.Users.ContainsKey(context.ClientRegistration.Id.ToString()))
            {
                next();
            }
            else if (string.IsNullOrWhiteSpace(context.Request.Body) == false)
            {
                if (
                    AlreadyRegistered() == false &&
                    Register()
                    )
                {
                    next();
                }
            }
            else
            {
                context.Response.Write("** Enter your nickname:");
            }


            return Task.CompletedTask;
        }

        private bool Register()
        {
            var nickName = _context.Request.Body;

            if(IsValid(nickName))
            {
                ChatInstance.Users.Add(_context.ClientRegistration.Id, nickName);

                _context.Notify($"** {nickName} has entered");

                return true;
            }
            else
            {
                _context.Response.WriteLine("** The nickname informed is invalid");
            }

            return false;

        }

        private bool IsValid(string nickName)
        {
            return !nickName.Contains(" ");
        }

        private bool AlreadyRegistered()
        {
            if(ChatInstance.Users.Values.Any(x => x.ToLower() == _context.Request.Body.ToLower()))
            {
                _context.Response.WriteLine("** The nickname is already registered");
                _context.Response.WriteLine("** Enter your nickname:");

                return true;
            }

            return false;
        }
    }

}

