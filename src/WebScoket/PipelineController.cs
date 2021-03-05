using System;
using System.Threading.Tasks;

namespace WebScoket
{
    public abstract class PipelineController : IPipeline
    {
        protected abstract string ActionName { get; }

        public Task ExecuteAsync(WebSocketContext context, Action next)
        {
            if (context.Request.Action.StartsWith(ActionName) == false)
                next.Invoke();


            return Task.CompletedTask;
        }

        protected void Response(string message, Privacy privacy = Privacy.Public)
        {

        }

        protected void Response(string message, Privacy privacy, ClientRegistration client)
        {
        
        }

    }
}
