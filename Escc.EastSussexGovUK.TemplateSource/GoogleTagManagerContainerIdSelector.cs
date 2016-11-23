using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.TemplateSource
{
    /// <summary>
    /// Selects the correct Google Tag Manager container id to use
    /// </summary>
    public class GoogleTagManagerContainerIdSelector
    {
        /// <summary>
        /// Selects the container id by matching the current hostname against a set of rules.
        /// </summary>
        /// <param name="hostName">The hostname for the current request.</param>
        /// <param name="rules">The rules, with regular expression keys and tag manager ids as values.</param>
        /// <returns></returns>
        public string SelectContainerId(string hostName, NameValueCollection rules)
        {
            var containerId = String.Empty;
            foreach (var rule in rules.AllKeys)
            {
                if (Regex.IsMatch(hostName, rule))
                {
                    containerId = rules[rule];
                    break;
                }
            }

            return containerId;
        }
    }
}