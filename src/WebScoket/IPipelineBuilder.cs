namespace WebScoket
{
    public interface IPipelineBuilder
    {
        IPipelineBuilder Use(IPipeline register);

        PipelineExecutor Build();
    }
}
