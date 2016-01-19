using System;
using System.Collections.Generic;
using System.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Gets the best breadcrumb trail provider for the current context
    /// </summary>
    /// <returns></returns>
    public static class BreadcrumbTrailFactory
    {
        /// <summary>
        /// Gets the best breadcrumb trail provider for the current context
        /// </summary>
        /// <returns></returns>
        public static IBreadcrumbProvider CreateTrailProvider()
        {
            return new ConfigurationBreadcrumbProvider();
        }
    }
}