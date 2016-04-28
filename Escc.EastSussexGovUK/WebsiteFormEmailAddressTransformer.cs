using System;
using System.Text;
using System.Web;
using Escc.AddressAndPersonalDetails;
using Escc.AddressAndPersonalDetails.Controls;

namespace EsccWebTeam.EastSussexGovUK
{
    /// <summary>
    /// Transforms an email address into the URL of a sitewide form used to send an email.
    /// </summary>
    /// <seealso cref="Escc.AddressAndPersonalDetails.Controls.IEmailAddressTransformer" />
    public class WebsiteFormEmailAddressTransformer : IEmailAddressTransformer
    {
        private readonly Uri _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteFormEmailAddressTransformer"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public WebsiteFormEmailAddressTransformer(Uri baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Transforms an email address into the URL of a sitewide form used to send an email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public string TransformEmailAddress(ContactEmail email)
        {
            if (email == null) throw new ArgumentNullException("email");
            return GetWebsiteEmailFormUri(email.EmailAddress, email.DisplayName, _baseUrl).ToString();
        }

        /// <summary>
        /// Gets a URI for sending an email without exposing an email address to spambots
        /// </summary>
        /// <param name="emailAddress">The email address</param>
        /// <param name="recipientName">The name of the email recipient</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns></returns>
        private static Uri GetWebsiteEmailFormUri(string emailAddress, string recipientName, Uri baseUrl)
        {
            if (string.IsNullOrEmpty(emailAddress)) return null;

            int atPos = emailAddress.IndexOf("@", StringComparison.Ordinal);
            if (atPos <= 0) return null;

            string emailAccount = emailAddress.Substring(0, atPos);
            string emailDomain = emailAddress.Substring(atPos + 1);

            return GetWebsiteEmailFormUri(emailAccount, emailDomain, recipientName, baseUrl);
        }

        /// <summary>
        /// Gets a URI for sending an email without exposing an email address to spambots
        /// </summary>
        /// <param name="emailAccount">The part of the email address before the @</param>
        /// <param name="emailDomain">The part of the email address after the @</param>
        /// <param name="recipientName">The name of the email recipient</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>
        /// URL of an ESCC website page, or null
        /// </returns>
        private static Uri GetWebsiteEmailFormUri(string emailAccount, string emailDomain, string recipientName, Uri baseUrl)
        {
            // Build up URL
            StringBuilder url = new StringBuilder(Uri.UriSchemeHttps)
                .Append("://")
                .Append(baseUrl.Authority)
                .Append("/contactus/emailus/email.aspx?n=")
                .Append(HttpUtility.UrlEncode(recipientName.Replace(" & ", " and ")))
                .Append("&e=")
                .Append(emailAccount)
                .Append("&d=")
                .Append(emailDomain);

            return new Uri(url.ToString());
        }
    }
}
