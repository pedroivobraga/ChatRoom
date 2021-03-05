using System;
using System.Threading.Tasks;

namespace WebScoket
{
    public class PipelineExecutor
    {
        private IPipeline _pipeline;
        private PipelineExecutor _next;

        internal PipelineExecutor(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }


        internal async Task RunAsync(WebSocketContext webSocketContext)
        {
            var onNext = new Action(async ()=>
            {
                if (this._next != null)
                    await this._next.RunAsync(webSocketContext);
            });

            await _pipeline.ExecuteAsync(webSocketContext, onNext);
        }

        internal void Attatch(PipelineExecutor executor)
        {
            _next = executor;
        }
    }
}
