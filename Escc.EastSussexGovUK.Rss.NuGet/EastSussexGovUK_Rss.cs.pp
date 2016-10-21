using System.Reflection;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.EastSussexGovUkRss), "PostStart")]

namespace $rootnamespace$ {

    /// <summary>
    /// Register the virtual path provider which makes available the embedded client files from Escc.EastSussexGovUK.Rss
    /// </summary>
    public static class EastSussexGovUkRss
	{
  		/// <summary>
		/// Wire up the provider at the end of the application startup process 
		/// </summary>
	    public static void PostStart() 
		{
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider.Vpp(typeof(Escc.EastSussexGovUK.Rss.RssToHtml).Assembly));
        }
    }
}