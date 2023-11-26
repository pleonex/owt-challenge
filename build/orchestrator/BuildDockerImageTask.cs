namespace BuildSystem;

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Core;
using Cake.Frosting;

[TaskName("BuildDockerImage")]
public class BuildDockerImageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetPublish(
            context.DockerWebProject,
            new DotNetPublishSettings {
                Configuration = context.DotNetContext.Configuration,
                OS = "linux",
                ArgumentCustomization = b => b
                    .AppendQuoted("--arch").AppendQuoted("x64")
                    .AppendQuoted("-p:PublishProfile=DefaultContainer")
                    .Append("-p").Append($"ContainerImageTags={context.Version}"),
            });
    }
}
