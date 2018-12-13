using System;
using System.Collections.Specialized;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// The text-size feature allows the user to change the text size independently of browser zoom
    /// </summary>
    public class TextSize : ITextSize
    {
        private readonly string _cookieValue;
        private readonly NameValueCollection _queryString;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextSize"/> class.
        /// </summary>
        /// <param name="cookieValue">The current value of the 'textsize' cookie.</param>
        /// <param name="queryString">The query string.</param>
        public TextSize(string cookieValue, NameValueCollection queryString)
        {
            _cookieValue = cookieValue;
            _queryString = queryString;
        }

        private int? _textSize;

        /// <summary>
        /// Gets the current user-selected text size 
        /// </summary>
        /// <returns></returns>
        public int CurrentTextSize()
        {
            // Return cached value if already worked out
            if (_textSize.HasValue) return _textSize.Value;

            // Look for text size value
            _textSize = 1;
            if (!String.IsNullOrWhiteSpace(_cookieValue))
            {
                try
                {
                    _textSize = Convert.ToInt32(_cookieValue);
                }
                catch (FormatException)
                {
                    // if value wrong, just ignore it
                }
                catch (OverflowException)
                {
                    // if value wrong, just ignore it
                }
            }

            // On the text size page itself, it could be in the querystring
            if (_queryString?["textsize"] != null)
            {
                try
                {
                    _textSize = Convert.ToInt32(_queryString["textsize"]);
                }
                catch (FormatException)
                {
                    // if value wrong, just ignore it
                }
                catch (OverflowException)
                {
                    // if value wrong, just ignore it
                }
            }

            // Only accept expected values
            if (_textSize < 1 || _textSize > 3) _textSize = 1;
            return _textSize.Value;
        }
    }
}