using System;
using System.Diagnostics.CodeAnalysis;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// A simple view model to enable the HTTP status pages to be displayed
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Core.BaseViewModel" />
    [ExcludeFromCodeCoverage]
    public class HttpStatusViewModel : BaseViewModel
    {
        /// <summary>
        /// Creates a new <see cref="HttpStatusViewModel"/>
        /// </summary>
        /// <param name="defaultValues">Provides essential context for views using the EastSussexGovUK template.</param>
        public HttpStatusViewModel(IViewModelDefaultValuesProvider defaultValues) : base(defaultValues) { }

        /// <summary>
        /// The correlation id for the request
        /// </summary>
        public string RequestId { get; set; }
    }
}