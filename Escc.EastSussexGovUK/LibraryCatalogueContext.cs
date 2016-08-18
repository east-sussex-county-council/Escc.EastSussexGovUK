using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// Detect whether the current request is from a library catalogue machine in a library, which has limited access
    /// </summary>
    public class LibraryCatalogueContext : ILibraryCatalogueContext
    {
        private readonly string _userAgent;
        private bool? _requestIsFromLibraryCatalogueMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryCatalogueContext" /> class.
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public LibraryCatalogueContext(string userAgent)
        {
            _userAgent = userAgent;
        }

        /// <summary>
        /// Gets whether the user is on a library catalogue machine in a library.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if user is on a library catalogue machine; otherwise, <c>false</c>.
        /// </value>
        public bool RequestIsFromLibraryCatalogueMachine()
        {
            if (this._requestIsFromLibraryCatalogueMachine == null)
            {
                this._requestIsFromLibraryCatalogueMachine = (_userAgent != null && _userAgent.IndexOf("ESCC Libraries", StringComparison.InvariantCulture) > -1);
            }

            return (bool)this._requestIsFromLibraryCatalogueMachine;
        }
    }
}
