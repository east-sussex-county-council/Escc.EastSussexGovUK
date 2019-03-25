using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Configuration settings for using Exceptionless to track errors
    /// </summary>
    public class ExceptionlessSettings
    {
        /// <summary>
        /// The API key for the project that exceptions should be logged to
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The base URL of the server where Exceptionless is being hosted, eg https://exceptionless.com
        /// </summary>
        public Uri ServerUrl { get; set; }
    }
}
