namespace BuildSystem;

using Cake.Core;
using Cake.Frosting.PleOps.Recipe;

public class BuildContext : PleOpsBuildContext
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
        SwaggerDocName = string.Empty;
        SwaggerAssemblyPath = string.Empty;
        SwaggerDocFxPath = "docs/rest/";
    }

    public string SwaggerDocName { get; set; }

    public string SwaggerAssemblyPath { get; set; }

    public string SwaggerDocFxPath { get; set; }
}
