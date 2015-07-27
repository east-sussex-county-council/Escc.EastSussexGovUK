using System;
using System.Collections.Generic;
using System.Web;
using EsccWebTeam.Cms;

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
            // Use CMS if installed, otherwise web.config
            if (CmsUtilities.IsCmsEnabled())
            {
                return new MicrosoftCmsBreadcrumbProvider();
            }
            else
            {
                return new ConfigurationBreadcrumbProvider();
            }
        }
    }
}