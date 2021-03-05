using System.Collections.Generic;
using System.Linq;

namespace WebScoket
{
    public class PipelineBuilder : IPipelineBuilder
    {
        private IList<PipelineExecutor> pipelines;

        public PipelineBuilder()
        {
            pipelines = new List<PipelineExecutor>();
        }


        public PipelineExecutor Build()
        {
            PipelineExecutor executor = pipelines.LastOrDefault();

            foreach (var component in pipelines.Reverse().Skip(1))
            {
                component.Attatch(executor);
                executor = component;
            }

            return executor;
        }

        public IPipelineBuilder Use(IPipeline register)
        {
            pipelines.Add(new PipelineExecutor(register));

            return this;
        }
    }
}
