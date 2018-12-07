using System;
using System.Web;
using System.Web.UI;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Allows master page to be switched based on querystring or URL path. See <see cref="ViewSelector"/>.
    /// </summary>
    public class MasterPageModule : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            //clean-up code here.
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            // Hook up to each request...
            context.PreRequestHandlerExecute += new System.EventHandler(context_PreRequestHandlerExecute);
        }

        #endregion

        /// <summary>
        /// Handles the PreRequestHandlerExecute event of the current application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // .. get the page and hook up to that...
            var page = HttpContext.Current.CurrentHandler as Page;
            if (page != null) page.PreInit += new EventHandler(page_PreInit);
        }

        /// <summary>
        /// Handles the PreInit event of the page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void page_PreInit(object sender, EventArgs e)
        {
            //... and change the master page.
            var page = sender as Page;

            // If there's no master page at the moment, this page would probably fail if we assigned a master page as it'll
            // have controls other than <asp:Content /> on it. So just drop out here in that case.
            if (String.IsNullOrEmpty(page.MasterPageFile)) return;

            var preferredMasterPage = ViewSelector.SelectView(page.Request.Url, page.Request.UserAgent, ViewEngine.WebForms);

            // If the master page has been set, change it. 
            // Otherwise if no default was in config, just leave it using the one it would've used anyway.
            if (!String.IsNullOrEmpty(preferredMasterPage))
            {
                page.MasterPageFile = preferredMasterPage;
            }
        }
    }
}
