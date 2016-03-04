using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.ActiveDirectory;
using Exceptionless.Extensions;

namespace EsccWebTeam.EastSussexGovUK
{
    /// <summary>
    /// Container control which shows or hides its contents depending on the current request context
    /// </summary>
    public class ContextContainer : PlaceHolder
    {
        #region Show contents depending on current master page

        /// <summary>
        /// Gets or sets whether to show contents on a desktop master page
        /// </summary>
        /// <value><c>true</c> to show on desktop; otherwise, <c>false</c>.</value>
        public bool? Desktop { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents on a mobile master page
        /// </summary>
        /// <value><c>true</c> to show on mobile; otherwise, <c>false</c>.</value>
        public bool? Mobile { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents on the legacy master page
        /// </summary>
        /// <value><c>true</c> to show on legacy; otherwise, <c>false</c>.</value>
        public bool? Legacy { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents on the plain master page.
        /// </summary>
        /// <value><c>true</c> to show on plain; otherwise, <c>false</c>.</value>
        public bool? Plain { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents on the full-screen master page.
        /// </summary>
        /// <value><c>true</c> to show on full-screen; otherwise, <c>false</c>.</value>
        public bool? FullScreen { get; set; }

        #endregion // Show contents depending on current master page

        #region Show contents depending on current user

        /// <summary>
        /// Gets or sets whether to show contents to library catalogue users in libraries.
        /// </summary>
        /// <value><c>true</c> to show on library catalogue PCs; otherwise, <c>false</c>.</value>
        public bool? LibraryCatalogue { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents to robots.
        /// </summary>
        /// <value><c>true</c> if robots; otherwise, <c>false</c>.</value>
        public bool? Robots { get; set; }

        /// <summary>
        /// Gets or sets the permitted security groups, separated by semicolons.
        /// </summary>
        /// <value>The groups.</value>
        public string Groups { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents if "Do Not Track" is set to OPT-OUT
        /// </summary>
        /// <value><c>true</c> to show content only to users who have opted out of tracking; <c>false</c> to show content to users who have opted in or not expressed a preference</value>
        public bool? DoNotTrack { get; set; }

        private bool contentsHidden = false;

        #endregion // Show contents depending on current user

        #region Show contents depending on location

        /// <summary>
        /// Show contents only if the request URL matches this regular expression
        /// </summary>
        public string UrlMatch { get; set; }

        /// <summary>
        /// Gets or sets whether to show content on a public-facing server.
        /// </summary>
        /// <value>Whether to show content.</value>
        public bool? Public { get; set; }

        #endregion // Show contents depending on location

        #region Show contents depending on date

        /// <summary>
        /// Gets or sets the date to show content before.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? Before { get; set; }

        /// <summary>
        /// Gets or sets the date to show content after
        /// </summary>
        /// <value>The start date.</value>
        public DateTime? After { get; set; }

        #endregion

        #region Show contents depending on CMS context

        /// <summary>
        /// Gets or sets whether to show content in CMS edit mode
        /// </summary>
        /// <value><c>true</c> to show content, <c>false</c> otherwise.</value>
        /// <remarks>CMS has its own controls to enable this, but they can only be used in a context where CMS is definitely available, eg on a CMS template, not the site header or footer</remarks>
        [Obsolete("Relates to Microsoft CMS 2002")]
        public bool? CmsEdit { get; set; }

        /// <summary>
        /// Gets or sets whether to show contents to people who can edit the site 
        /// </summary>
        /// <value><c>true</c> if user can edit site; otherwise <c>false</c></value>
        [Obsolete("Relates to Microsoft CMS 2002")]
        public bool? CmsPermission { get; set; }

        #endregion

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Ensure controls are visible by default
            this.Visible = true;

            // Look for various reasons why controls may need to be hidden
            var context = new EastSussexGovUKContext();

            // Hide based on master page
            if (Desktop != null && Desktop.Value != context.ViewIsDesktop)
            {
                HideContents();
                return;
            }

            if (Mobile != null && Mobile.Value != context.ViewIsMobile)
            {
                HideContents();
                return;
            }

            if (Plain != null && Plain.Value != context.ViewIsPlain)
            {
                HideContents();
                return;
            }

            if (Legacy != null && Legacy.Value != context.ViewIsLegacy)
            {
                HideContents();
                return;
            }

            if (FullScreen != null && FullScreen.Value != context.ViewIsFullScreen)
            {
                HideContents();
                return;
            }

            // Hide based on user
            if (LibraryCatalogue != null && LibraryCatalogue.Value != context.UserIsLibraryCatalogue)
            {
                HideContents();
                return;
            }

            if (Robots != null && Robots.Value != context.UserIsRobot)
            {
                HideContents();
                return;
            }

            if (!String.IsNullOrEmpty(Groups))
            {
                var settings = new ActiveDirectorySettingsFromConfiguration();
                var permissions = new LogonIdentityGroupMembershipChecker(settings.DefaultDomain, new SessionPermissionsResultCache());
                if ( !permissions.UserIsInGroup(Groups.SplitAndTrim(';')))
                {
                    HideContents();
                    return;
                }
            }

            // Test for Do Not Track is slightly different because if ContextContainer.DoNotTrack is false it must 
            // match a EastSussexGovUKContext.DoNotTrack set to either null or false
            if (DoNotTrack != null && ((DoNotTrack == true && context.DoNotTrack != true)
                                        || (DoNotTrack == false && context.DoNotTrack == true)))
            {
                HideContents();
                return;
            }

            // Hide based on location
            if (!String.IsNullOrEmpty(this.UrlMatch) && !Regex.IsMatch(context.RequestUrl.ToString(), this.UrlMatch, RegexOptions.IgnoreCase))
            {
                HideContents();
                return;
            }

            if (Public != null && Public.Value != context.IsPublicUrl)
            {
                HideContents();
                return;
            }

            // Hide based on date
            if (this.After.HasValue && DateTime.Now <= this.After)
            {
                HideContents();
                return;
            }

            if (this.Before.HasValue && DateTime.Now >= this.Before)
            {
                HideContents();
                return;
            }
        }

        /// <summary>
        /// Hides the controls contained by this control.
        /// </summary>
        private void HideContents()
        {
            // Hide all the child controls. It would make more sense to just set this.Visible = false, but that triggers
            // an ASP.NET bug. When this control immediately followed a ContentPlaceholder control on a master page, and 
            // then the child page triggered a postback, the ContentPlaceholder was also hidden. Not on every page though,
            // so there must be more to it than that. Rick Mason, 1 March 2011
            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }
            this.contentsHidden = true;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        /// <value></value>
        /// <returns>true if the control is visible on the page; otherwise false.
        /// </returns>
        public override bool Visible
        {
            get
            {
                // If calling code cares whether this control is visible, it actually cares whether
                // this control will hide its children, so return that answer.
                //
                // This code introduced because Escc.Web.Metadata.CombineStaticFilesControl would combine
                // a visible CSS or JavaScript file with one inside a ContextContainer that was destined to
                // become invisible later in the life cycle. 
                //
                // Haven't yet retested the bug documented in HideContents method though, to see whether it was
                // really the same thing happening and we could just be using base.Visible all along. 
                // Rick Mason, 24 May 2011
                EnsureChildControls();
                if (this.contentsHidden) return false;

                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}