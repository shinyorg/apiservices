namespace Shiny.Extensions.Webhooks;


public class WebHookRunnerConfig
{
    public int DefaultWaitTime { get; set; } = 30;
    public int MaxDegreeOfParallelism { get; set; } = 4;
}
