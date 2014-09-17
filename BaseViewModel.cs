
using System.Web;

namespace EsccWebTeam.EastSussexGovUK
{
    /// <summary>
    /// Base class for common properties to be available to all view models used on www.eastsussex.gov.uk
    /// </summary>
    public abstract class BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel()
        {
            IsPublicView = true;
        }
        /// <summary>
        /// Gets or sets the page title
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript for a Google Analytics content experiment
        /// </summary>
        public IHtmlString ContentExperimentScript { get; set; }

        /// <summary>
        /// Gets or sets whether the current view is a publicly-visible page
        /// </summary>
        public bool IsPublicView { get; set; }
    }
}
