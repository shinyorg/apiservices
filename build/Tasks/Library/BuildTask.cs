using Cake.Common.IO;
using Cake.Common.Tools.MSBuild;
using Cake.Frosting;


namespace ShinyBuild.Tasks.Library
{
    [TaskName("Build")]
    public sealed class BuildTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.CleanDirectories($"./src/**/obj/");
            context.CleanDirectories($"./src/**/bin/{context.MsBuildConfiguration}");

            context.MSBuild("Build.slnf", x => x
                .WithRestore()
                .WithTarget("Clean")
                .WithTarget("Build")
                .WithProperty("PublicRelease", "true")
                .WithProperty("CI", context.IsRunningInCI ? "true" : "")
                .SetConfiguration(context.MsBuildConfiguration)
            );
        }
    }
}