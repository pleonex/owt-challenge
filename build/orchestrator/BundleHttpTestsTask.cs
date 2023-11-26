namespace BuildSystem;

using Cake.Common.IO;
using Cake.Frosting;

[TaskName("BundleHttpTests")]
public class BundleHttpTestsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        string outputZip = Path.Combine(context.ArtifactsPath, "http-scripts.zip");
        context.Zip("./resources/system-tests", outputZip);
        context.DeliveriesContext.BinaryFiles.Add(outputZip);
    }
}