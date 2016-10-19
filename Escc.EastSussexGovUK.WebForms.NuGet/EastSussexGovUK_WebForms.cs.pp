[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.EastSussexGovUK_WebForms), "PostStart")]

namespace $rootnamespace$ {

    /// <summary>
    /// Register the virtual path provider which makes available the embedded views from Escc.EastSussexGovUK.Mvc
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    public static class EastSussexGovUK_WebForms
	{
  		/// <summary>
		/// Wire up the provider at the end of the application startup process 
		/// </summary>
	    public static void PostStart() 
		{
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider.Vpp(typeof(Escc.EastSussexGovUK.WebForms.BaseMasterPage).Assembly));
        }
    }
}