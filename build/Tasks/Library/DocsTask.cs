using Cake.DocFx;
using Cake.Frosting;
using ShinyBuild;


namespace ShinyBuild.Tasks
{
    public class DocsTask : FrostingTask<BuildContext>
    {
        //public override bool ShouldRun(BuildContext context)
        //{
        //    var result = context.IsNugetDeployBranch && context.IsRunningInCI;
        //    if (result && String.IsNullOrWhiteSpace(context.NugetApiKey))
        //        throw new ArgumentException("NugetApiKey is missing");

        //    return result;
        //}

        public override void Run(BuildContext context)
        {
            context.DocFxBuild("./docs/docfx.json");
        }
    }
}
