using System;
using System.Threading.Tasks;

namespace WebScoket
{
    public interface IPipeline
    {
        Task ExecuteAsync(WebSocketContext context, Action next);

    }
}
