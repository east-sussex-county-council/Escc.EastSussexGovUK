using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Switch the preferred view at the request of the user
    /// </summary>
    public class choose : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            ViewSelector.SwitchView(context.Request, context.Response, context.Session);
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}